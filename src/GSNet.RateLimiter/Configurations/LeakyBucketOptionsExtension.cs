using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GSNet.RateLimiter.Configurations;
using Microsoft.Extensions.DependencyInjection;

namespace GSNet.RateLimiter.Configurations
{
    /// <summary>
    /// 限流器选项的扩展--漏桶算法
    /// </summary>
    public class LeakyBucketOptionsExtension : IRateLimiterOptionsExtension
    {
        /// <summary>
        /// 限流器名称
        /// </summary>
        private readonly string _limiterName;

        /// <summary>
        /// 限流器配置信息
        /// </summary>
        private readonly Action<LeakyBucketOptions> _configure;

        public LeakyBucketOptionsExtension(string limiterName, Action<LeakyBucketOptions> configure)
        {
            _limiterName = limiterName;
            _configure = configure;
        }

        /// <summary>
        /// 添加  基于漏桶算法(匀速排队)的限流器服务
        /// </summary>
        public void AddServices(IServiceCollection services)
        {
            var options = new LeakyBucketOptions();
            _configure.Invoke(options);

            services.AddSingleton<IRateLimiter>(s => new LeakyBucketRateLimiter(_limiterName, options.Capacity, options.LeaksInterval));
        }
    }
}
