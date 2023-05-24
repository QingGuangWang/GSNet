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
    public class RateLimiterManager : IRateLimiterManager
    {
        private readonly IList<IRateLimiter> _rateLimiters;

        private readonly IDictionary<string, IRateLimiter> _rateLimitersDitc;

        public RateLimiterManager(IEnumerable<IRateLimiter> rateLimiters)
        {
            _rateLimiters = rateLimiters.ToList();

            _rateLimitersDitc = _rateLimiters?.ToDictionary(x => x.LimiterName) ?? new Dictionary<string, IRateLimiter>();
        }

        public IRateLimiter GetRateLimiter(string rateLimiterName)
        {
            if (_rateLimitersDitc.ContainsKey(rateLimiterName))
            {

                return _rateLimitersDitc[rateLimiterName];
            }

            throw new Exception($"A limiter named {rateLimiterName} could not be found.");
        }
    }
}
