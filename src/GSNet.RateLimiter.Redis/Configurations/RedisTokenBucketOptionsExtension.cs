using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GSNet.RateLimiter.Configurations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace GSNet.RateLimiter.Redis.Configurations
{
    /// <summary>
    /// 限流器选项的扩展--Redis令牌桶算法
    /// </summary>
    public class RedisTokenBucketOptionsExtension : IRateLimiterOptionsExtension
    {
        /// <summary>
        /// 限流器名称
        /// </summary>
        private readonly string _limiterName;

        /// <summary>
        /// 限流器配置信息
        /// </summary>
        private readonly Action<RedisTokenBucketOptions> _configure;

        public RedisTokenBucketOptionsExtension(string limiterName, Action<RedisTokenBucketOptions> configure)
        {
            _limiterName = limiterName;
            _configure = configure;
        }

        public void AddServices(IServiceCollection services)
        {
            var options = new RedisTokenBucketOptions();
            _configure.Invoke(options);

            services.AddSingleton<IRateLimiter>(s => new RedisTokenBucketRateLimiter(Options.Create(options.RedisConnectionOptions),
                _limiterName, options.PermitsPerSecond, options.MaxBurstSecond));
        }
    }
}
