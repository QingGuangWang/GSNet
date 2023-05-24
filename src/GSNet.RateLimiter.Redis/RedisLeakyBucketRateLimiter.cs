using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using GSNet.Common;
using GSNet.Json.SystemTextJson;
using GSNet.Json.SystemTextJson.Converters;
using GSNet.Redis.StackExchange;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace GSNet.RateLimiter.Redis
{
    public class RedisLeakyBucketRateLimiter : LeakyBucketRateLimiter
    {
        private readonly RedisConnectionOptions _redisConnectionOptions;

        private IDatabase _redisDatabase;

        private IConnectionMultiplexer _connectionMultiplexer;

        /// <summary>
        /// 分布式同步锁的key
        /// </summary>
        private readonly string _distributedSyncLockKey;

        /// <summary>
        /// 漏桶的存储在Redis的key
        /// </summary>
        private readonly string _leakyBucketKey;

        /// <summary>
        /// </summary> 
        public RedisLeakyBucketRateLimiter(IOptions<RedisConnectionOptions> options, string limiterName, int capacity, TimeSpan leaksInterval)
            : base(limiterName, capacity, leaksInterval)
        {
            Check.Argument.IsNotNullOrEmpty(limiterName, nameof(limiterName));

            _redisConnectionOptions = options.Value;

            _distributedSyncLockKey = $"DISTRIBUTED_LEAKY_BUCKET_LOCK_{limiterName}";
            _leakyBucketKey = $"LEAKY_BUCKET_KEY_{limiterName}";

            //初始化Redis
            InitRedisConnection();
            //初始化漏桶
            InitLeakyBucket(capacity, leaksInterval);
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
        /// 初始化漏桶
        /// </summary>
        public void InitLeakyBucket(int capacity, TimeSpan leaksInterval)
        {
            var obj = GetLeakyBucket();

            if (null == obj)
            {
                SetLeakyBucket(new LeakyBucket(capacity, leaksInterval));
            }
            //如果和数据库中的不匹配，则是调整了策略
            else if (obj.Capacity != capacity || obj.LeaksInterval != leaksInterval)
            {
                SetLeakyBucket(new LeakyBucket(capacity, leaksInterval));
            }
        }

        /// <summary>
        /// 获取漏桶
        /// </summary>
        protected override LeakyBucket GetLeakyBucket()
        {
            var redisResult = _redisDatabase.StringGet(_leakyBucketKey);

            if (redisResult == RedisValue.Null)
            {
                return default(LeakyBucket);
            }


            return JsonSerializer.Deserialize<LeakyBucket>(redisResult, GetJsonSerializerOptions());
        }

        /// <summary>
        /// 设置漏桶
        /// </summary>
        /// <param name="permits"></param>
        protected override void SetLeakyBucket(LeakyBucket permits)
        {
            //直接使用System.Text.Json 序列化
            var jsonString = JsonSerializer.Serialize<LeakyBucket>(permits, GetJsonSerializerOptions());

            //_redisDatabase.StringSet(_LeakyBucketKey, jsonString, new TimeSpan(0, 0, (int)permits.GetExpires()));
            _redisDatabase.StringSet(_leakyBucketKey, jsonString, null); //漏桶不删除
        }

        protected JsonSerializerOptions GetJsonSerializerOptions()
        {
            return JsonSerializerOptionsHelper.GetOrCreateJsonSerializerOptions("RedisLeakyBucketJsonSerializerOptions", () =>
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
