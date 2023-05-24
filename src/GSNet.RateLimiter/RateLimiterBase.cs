using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GSNet.RateLimiter
{
    public abstract class RateLimiterBase : IRateLimiter
    {
        /// <summary>
        /// </summary>
        protected RateLimiterBase(string limiterName)
        {
            LimiterName = limiterName;
        }

        public string LimiterName { get; private set; }

        /// <summary>
        /// 获取许可证
        /// </summary>
        /// <returns>返回获取许可证的结果</returns>
        public virtual AcquirePermitResult AcquirePermit()
        {
            return AcquirePermits(1);
        }

        /// <summary>
        /// 获取指定数量的许可证，直到获取到为止
        /// </summary>
        /// <param name="permits">需要获取的许可证数量</param>
        /// <returns>返回获取许可证的结果</returns>
        public virtual AcquirePermitResult AcquirePermits(int permits)
        {
            return AcquirePermits(1, TimeSpan.Zero);
        }

        /// <summary>
        /// 获取指定数量的许可证
        /// </summary>
        /// <param name="permits">需要获取的许可证数量</param>
        /// <param name="timeout">超时时间</param>
        /// <returns>返回获取许可证的结果</returns>
        public abstract AcquirePermitResult AcquirePermits(int permits, TimeSpan timeout);

        /// <summary>
        /// 异步方式获取许可证，直到获取到为止
        /// </summary>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>返回获取许可证的结果</returns>
        public virtual Task<AcquirePermitResult> AcquirePermitAsync(CancellationToken cancellationToken = default)
        {
            return AcquirePermitsAsync(1, cancellationToken);
        }

        /// <summary>
        /// 异步方式获取指定数量的许可证，直到获取到为止
        /// </summary>
        /// <param name="permits">需要获取的许可证数量</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>返回获取许可证的结果</returns>
        public virtual Task<AcquirePermitResult> AcquirePermitsAsync(int permits, CancellationToken cancellationToken = default)
        {
            return AcquirePermitsAsync(1, TimeSpan.Zero, cancellationToken);
        }

        /// <summary>
        /// 异步方式获取指定数量的许可证
        /// </summary>
        /// <param name="permits">需要获取的许可证数量</param>
        /// <param name="timeout"></param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>返回获取许可证的结果</returns>
        public abstract Task<AcquirePermitResult> AcquirePermitsAsync(int permits, TimeSpan timeout,
            CancellationToken cancellationToken = default);

    }
}
