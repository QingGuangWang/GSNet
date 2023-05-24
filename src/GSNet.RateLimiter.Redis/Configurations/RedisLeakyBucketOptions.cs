using GSNet.RateLimiter.Configurations;
using GSNet.Redis.StackExchange;

namespace GSNet.RateLimiter.Redis.Configurations
{
    /// <summary>
    /// 基于Redis的漏桶限流选项
    /// </summary>
    public class RedisLeakyBucketOptions : LeakyBucketOptions
    {
        public RedisLeakyBucketOptions()
        {
            RedisConnectionOptions = new RedisConnectionOptions();
        }

        /// <summary>
        /// Redis连接选项
        /// </summary>
        public RedisConnectionOptions RedisConnectionOptions { get; set; }
    }
}
