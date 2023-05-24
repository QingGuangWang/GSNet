using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSNet.RateLimiter.Configurations
{
    /// <summary>
    /// 令牌桶选项
    /// </summary>
    public class TokenBucketOptions
    {
        /// <summary>
        /// 每一秒生成的许可令牌
        /// 若值为5，表示每秒生成5个。
        /// 若值为0.5， 表示每 2秒 生成 1个。
        /// </summary>
        public decimal PermitsPerSecond { get; set; }

        /// <summary>
        /// 最大存储多少时间内生成的令牌， 单位是秒。
        /// 若值为5，则表示最多存储5秒内生成的令牌。
        /// </summary>
        public decimal MaxBurstSecond { get; set; }
    }
}
