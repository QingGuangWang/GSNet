using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GSNet.Common;
using GSNet.RateLimiter.Configurations;

namespace GSNet.RateLimiter.Configurations
{
    /// <summary>
    /// 限流器选项扩展
    /// </summary>
    public static class RateLimiterOptionsExtensions
    {
        /// <summary>
        /// 添加基于漏桶算法(匀速排队)的限流器
        /// </summary>
        public static RateLimiterOptions AddLeakyBucketRateLimiter(this RateLimiterOptions options,
            Action<LeakyBucketOptions> configure, string limiterName)
        {
            Check.Argument.IsNotNull(configure, nameof(configure));

            options.RegisterExtension(new LeakyBucketOptionsExtension(limiterName, configure));
            return options;
        }

        /// <summary>
        /// 添加基于令牌桶算法的限流器
        /// </summary>
        public static RateLimiterOptions AddTokenBucketRateLimiter(this RateLimiterOptions options,
            Action<TokenBucketOptions> configure, string limiterName)
        {
            Check.Argument.IsNotNull(configure, nameof(configure));

            options.RegisterExtension(new TokenBucketOptionsExtension(limiterName, configure));
            return options;
        }
    }
}
