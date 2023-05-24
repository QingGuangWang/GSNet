using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GSNet.Common;
using GSNet.RateLimiter.Configurations;
using GSNet.RateLimiter.Redis.Configurations;
using GSNet.RateLimiter.Redis.Configurations;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class RateLimiterOptionsExtensions
    {
        public static RateLimiterOptions AddRedisLeakyBucketRateLimiter(this RateLimiterOptions options,
            Action<RedisLeakyBucketOptions> configure, string limiterName)
        {
            Check.Argument.IsNotNull(configure, nameof(configure));

            options.RegisterExtension(new RedisLeakyBucketOptionsExtension(limiterName, configure));
            return options;
        }

        public static RateLimiterOptions AddRedisTokenBucketRateLimiter(this RateLimiterOptions options,
            Action<RedisTokenBucketOptions> configure, string limiterName)
        {
            Check.Argument.IsNotNull(configure, nameof(configure));

            options.RegisterExtension(new RedisTokenBucketOptionsExtension(limiterName, configure));
            return options;
        }
    }
}
