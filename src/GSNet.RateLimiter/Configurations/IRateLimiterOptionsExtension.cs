using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace GSNet.RateLimiter.Configurations
{
    /// <summary>
    /// 限流器选项的扩展
    /// </summary>
    public interface IRateLimiterOptionsExtension
    {
        /// <summary>
        /// 添加服务
        /// </summary>
        /// <param name="services">Services.</param>
        void AddServices(IServiceCollection services);
    }
}
