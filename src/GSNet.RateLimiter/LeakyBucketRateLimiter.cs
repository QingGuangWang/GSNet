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
    /// 基于漏桶算法(匀速排队)的限流器
    /// </summary>
    public class LeakyBucketRateLimiter : RateLimiterBase
    {
        /// <summary>
        /// 最近一次漏的时间戳
        /// </summary>
        private readonly long _lastLeakTimestamp;

        /// <summary>
        /// 漏桶
        /// </summary>
        private readonly LeakyBucket _leakyBucket;

        /// <summary>
        /// 同步锁
        /// </summary>
        private readonly object _lockObject = new object();

        /// <summary>
        /// </summary> 
        public LeakyBucketRateLimiter(string limiterName, int capacity, TimeSpan leaksInterval) : base(limiterName)
        {
            //初始化桶
            _leakyBucket = new LeakyBucket(capacity, leaksInterval);
        }

        /// <summary>
        /// 获取指定数量的许可证
        /// </summary>
        /// <param name="requiredPermits">需要获取的许可证数量</param>
        /// <param name="timeout">超时时间</param>
        /// <returns>返回获取许可证的结果</returns>
        public override AcquirePermitResult AcquirePermits(int requiredPermits, TimeSpan timeout)
        {
            Check.Argument.IsNotNegativeOrZero(requiredPermits, nameof(requiredPermits));

            AcquirePermitResult result;

            try
            {
                this.ApplyLock();

                result = AcquirePermitsCore(requiredPermits, timeout);
            }
            finally
            {
                this.ReleaseLock();
            }

            if (result.Succeed && result.WaitingTimeForSuccess > TimeSpan.Zero)
            {
                Thread.Sleep(result.WaitingTimeForSuccess);
            }

            return result;
        }
        
        public override Task<AcquirePermitResult> AcquirePermitsAsync(int permits, TimeSpan timeout, CancellationToken cancellationToken = default)
        {
            return Task.Run(() => AcquirePermits(permits, timeout), cancellationToken);
        }

        /// <summary>
        /// 获取指定数量的许可证的核心方法
        /// </summary> 
        protected AcquirePermitResult AcquirePermitsCore(int requiredPermits, TimeSpan timeout)
        {
            //当前时间戳
            var nowTimestamp = Stopwatch.GetTimestamp();

            var leakyBucket = this.GetLeakyBucket();
            leakyBucket.ReSync(nowTimestamp);

            //如果总和 超出了桶的容量, 即溢出，则申请许可证失败。
            if ((leakyBucket.Count + requiredPermits) > leakyBucket.Capacity)
            {
                return new AcquirePermitResult()
                {
                    Succeed = false
                };
            }

            //正常获取许可证所需要等待的时间
            var waitTimeForRequiredPermits = leakyBucket.LeaksInterval.Multiply(requiredPermits);

            //如果最后一滴留出至今的时间间隔 大于 漏桶漏水的时间间隔
            if ((leakyBucket.LastLeakTimestamp + leakyBucket.LeaksInterval.Ticks) < nowTimestamp)
            {
                //先调整为 当前时间 减去 漏桶漏水的时间间隔， 避免由于上一次限流至今过久，而导致后续计算获取多个（大于1个）许可的时候，都不需要等待。
                //这样子，如果只需要获取一个许可证的时候，计算后 最后一滴漏出的时间 就是当前事件。
                leakyBucket.LastLeakTimestamp = nowTimestamp - leakyBucket.LeaksInterval.Ticks;
            }

            //总共需要等待的时间
            TimeSpan needWaitingTime;

            //更新最后一次漏水需要的时间
            leakyBucket.LastLeakTimestamp = leakyBucket.LastLeakTimestamp + waitTimeForRequiredPermits.Ticks;

            if (leakyBucket.LastLeakTimestamp <= nowTimestamp)
            {
                leakyBucket.LastLeakTimestamp = nowTimestamp;
                needWaitingTime = TimeSpan.Zero;
            }
            else
            {
                //总共需要等待的时间， 计算两个时间的间隔
                var momentAvailable = DateTimeHelper.ParseDuration(nowTimestamp, leakyBucket.LastLeakTimestamp);
                needWaitingTime = momentAvailable.Ticks > 0 ? momentAvailable : TimeSpan.Zero;
            }

            //请求数量没有溢出，但是等待事件超过上层调用者指定的时间
            if (timeout != TimeSpan.Zero && timeout > needWaitingTime)
            {
                return new AcquirePermitResult()
                {
                    Succeed = false
                };
            }

            //保存楼桶信息
            SetLeakyBucket(leakyBucket);

            return new AcquirePermitResult()
            {
                Succeed = true,
                WaitingTimeForSuccess = needWaitingTime
            };
        }

        #region 漏桶

        /// <summary>
        /// 获取令牌桶
        /// </summary>
        protected virtual LeakyBucket GetLeakyBucket()
        {
            return _leakyBucket;
        }

        /// <summary>
        /// 设置令牌桶
        /// </summary>
        /// <param name="tokenBucket">令牌桶对象</param>
        protected virtual void SetLeakyBucket(LeakyBucket tokenBucket)
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
