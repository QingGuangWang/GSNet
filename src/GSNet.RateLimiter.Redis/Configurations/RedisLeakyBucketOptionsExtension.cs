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
    /// 限流器选项的扩展--Redis漏桶算法
    /// </summary>
    public class RedisLeakyBucketOptionsExtension : IRateLimiterOptionsExtension
    {
        /// <summary>
        /// 限流器名称
        /// </summary>
        private readonly string _limiterName;

        /// <summary>
        /// 限流器配置信息
        /// </summary>
        private readonly Action<RedisLeakyBucketOptions> _configure;

        public RedisLeakyBucketOptionsExtension(string limiterName, Action<RedisLeakyBucketOptions> configure)
        {
            _limiterName = limiterName;
            _configure = configure;
        }

        public void AddServices(IServiceCollection services)
        {
            var options = new RedisLeakyBucketOptions();
            _configure.Invoke(options);

            services.AddSingleton<IRateLimiter>(s => new RedisLeakyBucketRateLimiter(Options.Create(options.RedisConnectionOptions),
                _limiterName, options.Capacity, options.LeaksInterval));
        }
    }
}
