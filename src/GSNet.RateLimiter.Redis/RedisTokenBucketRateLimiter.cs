using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using GSNet.Common;
using GSNet.Json.SystemTextJson;
using GSNet.Json.SystemTextJson.Converters;
using GSNet.Redis.StackExchange;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace GSNet.RateLimiter.Redis
{
    /// <summary>
    /// 基于Redis 实现的令牌桶算法的分布式限流器
    /// </summary>
    public class RedisTokenBucketRateLimiter : TokenBucketRateLimiter
    {
        private readonly RedisConnectionOptions _redisConnectionOptions;

        private IDatabase _redisDatabase;

        private IConnectionMultiplexer _connectionMultiplexer;

        /// <summary>
        /// 分布式同步锁的key
        /// </summary>
        private readonly string _distributedSyncLockKey;

        /// <summary>
        /// 令牌桶的存储在Redis的key
        /// </summary>
        private readonly string _tokenBucketKey;

        /// <summary>
        /// </summary> 
        public RedisTokenBucketRateLimiter(IOptions<RedisConnectionOptions> options, string rateLimiterName, decimal permitsPerSecond, decimal maxBurstSecond = 1) 
            : base(rateLimiterName, permitsPerSecond, maxBurstSecond)
        {
            Check.Argument.IsNotNullOrEmpty(rateLimiterName, nameof(rateLimiterName));

            _redisConnectionOptions = options.Value;

            _distributedSyncLockKey = $"DISTRIBUTED_TOKEN_BUCKET_LOCK_{rateLimiterName}";
            _tokenBucketKey = $"TOKEN_BUCKET_KEY_{rateLimiterName}";

            //初始化Redis
            InitRedisConnection();
            //初始化令牌桶
            InitTokenBucket(permitsPerSecond, maxBurstSecond);
        }

        /// <summary>
        /// 初始化Redis
        /// </summary>
        public void InitRedisConnection()
        {
            _connectionMultiplexer = RedisConnectionHelp.Connect(_redisConnectionOptions);
            _redisDatabase = _connectionMultiplexer.GetDatabase();
        }

        /// <summary>
        /// 初始化令牌桶
        /// </summary>
        public void InitTokenBucket(decimal permitsPerSecond, decimal maxBurstSecond)
        {
            var obj = GetTokenBucket();
            
            if (null == obj)
            {
                 SetTokenBucket(new TokenBucket(permitsPerSecond, maxBurstSecond));
            }
            //如果和数据库中的不匹配，则是调整了策略
            else if(obj.PermitsPerSecond != permitsPerSecond || obj.MaxBurstSecond != maxBurstSecond)
            {
                 SetTokenBucket(new TokenBucket(permitsPerSecond, maxBurstSecond));
            }
        }

        /// <summary>
        /// 获取令牌桶
        /// </summary>
        protected override TokenBucket GetTokenBucket()
        {
            var redisResult = _redisDatabase.StringGet(_tokenBucketKey);

            if (redisResult == RedisValue.Null)
            {
                return default(TokenBucket);
            }

            return JsonSerializer.Deserialize<TokenBucket>(redisResult, GetJsonSerializerOptions());
        }

        /// <summary>
        /// 设置令牌桶
        /// </summary>
        /// <param name="permits"></param>
        protected override void SetTokenBucket(TokenBucket permits)
        {
            //直接使用System.Text.Json 序列化
            var jsonString = JsonSerializer.Serialize<TokenBucket>(permits, GetJsonSerializerOptions());

            //_redisDatabase.StringSet(_tokenBucketKey, jsonString, new TimeSpan(0, 0, (int)permits.GetExpires()));
            _redisDatabase.StringSet(_tokenBucketKey, jsonString, null); //令牌桶不删除
        }

        protected JsonSerializerOptions GetJsonSerializerOptions()
        {
            return JsonSerializerOptionsHelper.GetOrCreateJsonSerializerOptions("RedisTokenBucketJsonSerializerOptions", () =>
            {
                var jsonSerializerOptions = JsonSerializerOptionsHelper.CreateDefaultJsonSerializerOptions();
                jsonSerializerOptions.Converters.Add(new JsonTimeSpanConverter());

                return jsonSerializerOptions;
            });
        }

        /// <summary>
        /// 申请加锁 
        /// </summary>
        /// <param name="timeoutForMillisecond">申请锁超时时间（单位：毫秒）， 默认10秒</param>
        protected override bool ApplyLock(int timeoutForMillisecond = 10000)
        {
            var beginTime = DateTime.UtcNow;

            while ((DateTime.UtcNow - beginTime).TotalMilliseconds <= timeoutForMillisecond)
            {
                if (_redisDatabase.LockTake(_distributedSyncLockKey, $"{_distributedSyncLockKey}_Value", new TimeSpan(0, 0, 30)))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 释放锁
        /// </summary>
        protected override void ReleaseLock()
        {
            _redisDatabase.LockRelease(_distributedSyncLockKey, $"{_distributedSyncLockKey}_Value");
        }
    }
}
