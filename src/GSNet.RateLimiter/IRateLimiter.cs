namespace GSNet.RateLimiter
{
    /// <summary>
    /// 限流器
    /// </summary>
    public interface IRateLimiter
    {
        /// <summary>
        /// 限流器名称
        /// </summary>
        string LimiterName { get; }

        /// <summary>
        /// 获取许可证
        /// </summary>
        /// <returns>返回获取许可证的结果</returns>
        AcquirePermitResult AcquirePermit();

        /// <summary>
        /// 获取指定数量的许可证，直到获取到为止
        /// </summary>
        /// <param name="permits">需要获取的许可证数量</param>
        /// <returns>返回获取许可证的耗时</returns>
        AcquirePermitResult AcquirePermits(int permits);

        /// <summary>
        /// 获取指定数量的许可证
        /// </summary>
        /// <param name="permits">需要获取的许可证数量</param>
        /// <param name="timeout">超时时间</param>
        /// <returns>返回获取许可证的耗时</returns>
        AcquirePermitResult AcquirePermits(int permits, TimeSpan timeout);

        /// <summary>
        /// 异步方式获取许可证，直到获取到为止
        /// </summary>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>返回获取许可证的耗时</returns>
        Task<AcquirePermitResult> AcquirePermitAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// 异步方式获取指定数量的许可证，直到获取到为止
        /// </summary>
        /// <param name="permits">需要获取的许可证数量</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>返回获取许可证的耗时</returns>
        Task<AcquirePermitResult> AcquirePermitsAsync(int permits, CancellationToken cancellationToken = default);

        /// <summary>
        /// 异步方式获取指定数量的许可证
        /// </summary>
        /// <param name="permits">需要获取的许可证数量</param>
        /// <param name="timeout"></param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>返回获取许可证的耗时</returns>
        Task<AcquirePermitResult> AcquirePermitsAsync(int permits, TimeSpan timeout, CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// 获取许可的结果
    /// </summary>
    public class AcquirePermitResult
    {
        /// <summary>
        /// 是否获取成功
        /// </summary>
        public bool Succeed { get; set; }

        /// <summary>
        /// 获取成功，所等待的时长
        /// </summary>
        public TimeSpan WaitingTimeForSuccess { get; set; }

        /// <summary>
        /// 间隔时间
        /// </summary>
        public TimeSpan MomentAvailableInterval { get; set; }
    }
}