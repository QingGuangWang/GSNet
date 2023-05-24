using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace GSNet.RateLimiter.Configurations
{
    /// <summary>
    /// 限流器选项的扩展--令牌桶算法
    /// </summary>
    public class TokenBucketOptionsExtension : IRateLimiterOptionsExtension
    {
        /// <summary>
        /// 限流器名称
        /// </summary>
        private readonly string _limiterName;

        /// <summary>
        /// 限流器配置信息
        /// </summary>
        private readonly Action<TokenBucketOptions> _configure;

        /// <summary>
        /// </summary> 
        public TokenBucketOptionsExtension(string limiterName, Action<TokenBucketOptions> configure)
        {
            _limiterName = limiterName;
            _configure = configure;
        }

        /// <summary>
        /// 添加 基于令牌桶算法的限流器服务
        /// </summary>
        public void AddServices(IServiceCollection services)
        {
            var options = new TokenBucketOptions();
            _configure.Invoke(options);

            services.AddSingleton<IRateLimiter>(s => new TokenBucketRateLimiter(_limiterName, options.PermitsPerSecond, options.MaxBurstSecond));
        }
    }
}
