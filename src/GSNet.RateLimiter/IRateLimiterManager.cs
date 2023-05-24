using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSNet.RateLimiter
{
    /// <summary>
    /// 限流器管理器
    /// </summary>
    public interface IRateLimiterManager
    {
        /// <summary>
        /// 获取限流器
        /// </summary>
        /// <param name="rateLimiterName">限流器名称</param>
        /// <returns></returns>
        IRateLimiter GetRateLimiter(string rateLimiterName);
    }
}
