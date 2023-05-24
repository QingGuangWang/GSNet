using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GSNet.Common;
using GSNet.Common.Helper;

namespace GSNet.RateLimiter
{
    /// <summary>
    /// 基于令牌桶算法的限流器
    /// <para>
    /// 该实现里面，令牌桶的令牌数量是通过访问的时间差来动态计算，而不是通过一个线程在追加令牌。
    /// 该算法，参考的是guava的RateLimiter，当前请求的债务（请求的令牌大于限流器存储的令牌数）由下一个请求来偿还（上个请求亏欠的令牌，下个请求需要等待亏欠令牌生产出来以后才能被授权）
    /// </para>
    /// </summary>
    public class TokenBucketRateLimiter : RateLimiterBase
    {
        /// <summary>
        /// 令牌桶
        /// </summary>
        private readonly TokenBucket _tokenBucket;

        /// <summary>
        /// 同步锁
        /// </summary>
        private readonly object _lockObject = new object();

        /// <summary>
        /// </summary> 
        public TokenBucketRateLimiter(string rateLimiterName, decimal permitsPerSecond, decimal maxBurstSecond) : base(rateLimiterName)
        {
            _tokenBucket = new TokenBucket(permitsPerSecond, maxBurstSecond);
        }

        /// <summary>
        /// 获取指定数量的许可证
        /// </summary>
        /// <param name="permits">需要获取的许可证数量</param>
        /// <param name="timeout">超时时间</param>
        /// <returns>返回获取许可证的结果</returns>
        public override AcquirePermitResult AcquirePermits(int permits, TimeSpan timeout)
        {
            Check.Argument.IsNotNegativeOrZero(permits, nameof(permits));

            //如果没有 指定 超时时间
            if (timeout == TimeSpan.Zero)
            {
                var waitTimeout = Reserve(permits);
                
                if (waitTimeout != TimeSpan.Zero)
                {
                    Thread.Sleep(waitTimeout);
                }
                
                return new AcquirePermitResult()
                {
                    Succeed = true
                };
            }
            else
            {
                TimeSpan waitTimeout;

                try
                {
                    this.ApplyLock();

                    var nowTimestamp = Stopwatch.GetTimestamp();
                    if (!CanAcquire(permits, nowTimestamp, timeout, out var momentAvailableInterval))
                    {
                        return new AcquirePermitResult
                        {
                            Succeed = false,
                            MomentAvailableInterval = momentAvailableInterval
                        };
                    }
                    else
                    {
                        waitTimeout = ReserveAndGetWaitLength(permits, nowTimestamp);
                    }
                }
                finally
                {
                    this.ReleaseLock();
                }

                //await _asyncBlocker.WaitAsync(waitTimeout, cancellationToken);
                Thread.Sleep(waitTimeout);

                return new AcquirePermitResult
                {
                    Succeed = true,
                    MomentAvailableInterval = TimeSpan.Zero
                };
            }
        }

        /// <summary>
        /// 异步方式获取指定数量的许可证
        /// </summary>
        /// <param name="permits">需要获取的许可证数量</param>
        /// <param name="timeout">超时时间</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>返回获取许可证的结果</returns>
        public override Task<AcquirePermitResult> AcquirePermitsAsync(int permits, TimeSpan timeout, CancellationToken cancellationToken = default)
        {
            return Task.Run(() => AcquirePermits(permits, timeout), cancellationToken);
        }

        /// <summary>
        /// 获取令牌{permits}个需要等待的时间
        /// </summary>
        /// <param name="permits">令牌数</param>
        protected TimeSpan Reserve(int permits)
        {
            if (!(permits > 0))
            {
                throw new ArgumentOutOfRangeException(nameof(permits));
            }

            try
            {
                this.ApplyLock();
                return ReserveAndGetWaitLength(permits, Stopwatch.GetTimestamp());
            }
            finally
            {
                this.ReleaseLock();
            }
        }

        /// <summary>
        /// 返回获取{permits}个令牌最早可用的时间
        /// </summary>
        /// <param name="permits">令牌数</param>
        /// <param name="nowTimestamp">当前时间戳</param>
        private long QueryEarliestAvailable(long permits, long nowTimestamp)
        {
            if (nowTimestamp <= 0)
            {
                nowTimestamp = Stopwatch.GetTimestamp();
            }

            var permit = this.GetTokenBucket();
            permit.ReSync(nowTimestamp);

            //可以消耗的令牌数
            var storedPermitsToSpend = Math.Min(permits, permit.StoredPermits);
            // 需要等待的令牌数
            var freshPermits = permits - storedPermitsToSpend;
            // 需要等待的时间
            var waitTimeout = permit.StableInterval.Multiply((double)freshPermits);

            return DateTimeHelper.GetNextTimestamp(permit.NextFreeTicketTimestamp, waitTimeout);
        }

        protected TimeSpan ReserveAndGetWaitLength(int permits, long nowTimestamp)
        { 
            //计算两个时间的间隔
            var momentAvailable = DateTimeHelper.ParseDuration(nowTimestamp, ReserveEarliestAvailable(permits, nowTimestamp));
            return momentAvailable.Ticks > 0 ? momentAvailable : TimeSpan.Zero;
        }

        /// <summary>
        /// 预定指定个数（<paramref name="requiredPermits"/>）的许可证数(令牌数), 并返回所需要等待的时间
        /// </summary>
        /// <param name="requiredPermits">要求的许可证数(令牌数)</param>
        /// <param name="nowTimestamp">当前时间戳</param>
        /// <returns></returns>
        protected long ReserveEarliestAvailable(long requiredPermits, long nowTimestamp)
        {
            if (nowTimestamp <= 0)
            {
                nowTimestamp = Stopwatch.GetTimestamp();
            }

            var permit = this.GetTokenBucket();
            permit.ReSync(nowTimestamp);

            var returnValue = permit.NextFreeTicketTimestamp;
            //可以消耗的令牌数
            var storedPermitsToSpend = Math.Min(requiredPermits, permit.StoredPermits);
            // 需要等待的令牌数
            var freshPermits = requiredPermits - storedPermitsToSpend;
            // 需要等待的时间
            var waitTimeout = permit.StableInterval.Multiply((double)freshPermits);

            permit.NextFreeTicketTimestamp = DateTimeHelper.GetNextTimestamp(permit.NextFreeTicketTimestamp, waitTimeout);
            permit.StoredPermits -= storedPermitsToSpend;

            SetTokenBucket(permit);
            return returnValue;
        }

        /// <summary>
        /// 在等待的时间内是否可以获取到令牌
        /// </summary>
        /// <param name="permits">令牌数</param>
        /// <param name="nowTimestamp">当前时间戳</param>
        /// <param name="timeout">超时时间</param>
        /// <param name="momentAvailableInterval">间隔时间</param>
        protected bool CanAcquire(long permits, long nowTimestamp, TimeSpan timeout, out TimeSpan momentAvailableInterval)
        {
            momentAvailableInterval = DateTimeHelper.ParseDuration(nowTimestamp, QueryEarliestAvailable(permits, nowTimestamp));
            return momentAvailableInterval <= timeout;
        }

        #region 令牌桶

        /// <summary>
        /// 获取令牌桶
        /// </summary>
        protected virtual TokenBucket GetTokenBucket()
        {
            return _tokenBucket;
        }

        /// <summary>
        /// 设置令牌桶
        /// </summary>
        /// <param name="tokenBucket">令牌桶对象</param>
        protected virtual void SetTokenBucket(TokenBucket tokenBucket)
        {
            
        }
        
        #endregion

        /// <summary>
        /// 申请加锁 
        /// </summary>
        /// <param name="timeoutForMillisecond">申请锁超时时间（单位：毫秒）， 默认10秒</param>
        protected virtual bool ApplyLock(int timeoutForMillisecond = 10000)
        {
            Monitor.Enter(_lockObject);
            return true;
        }

        /// <summary>
        /// 释放锁
        /// </summary>
        protected virtual void ReleaseLock()
        {
            Monitor.Exit(_lockObject);
        }

    }
}
