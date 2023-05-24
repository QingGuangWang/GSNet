namespace GSNet.RateLimiter.Configurations
{
    /// <summary>
    /// 漏桶选项
    /// </summary>
    public class LeakyBucketOptions
    {
        /// <summary>
        /// 漏桶的容量
        /// </summary>
        public int Capacity { get; set; }

        /// <summary>
        /// 漏桶漏水的时间间隔
        /// </summary>
        public TimeSpan LeaksInterval { get; set; }
    }
}
