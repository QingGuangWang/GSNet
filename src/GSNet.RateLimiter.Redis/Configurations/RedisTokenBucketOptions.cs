using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GSNet.RateLimiter.Configurations;
using GSNet.Redis.StackExchange;

namespace GSNet.RateLimiter.Redis.Configurations
{
    /// <summary>
    /// 基于Redis的令牌桶限流选项
    /// </summary>
    public class RedisTokenBucketOptions : TokenBucketOptions
    {
        public RedisTokenBucketOptions()
        {
            RedisConnectionOptions = new RedisConnectionOptions();
        }

        /// <summary>
        /// Redis连接选项
        /// </summary>
        public RedisConnectionOptions RedisConnectionOptions { get; set; }
    }
}
