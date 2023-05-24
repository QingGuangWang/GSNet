using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSNet.RateLimiter
{
    /// <summary>
    /// 表示一个令牌桶
    /// </summary>
    [Serializable]
    public class TokenBucket
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

        /// <summary>
        /// 最大允许存在的许可令牌
        /// </summary>
        public decimal MaxPermits { get; set; }

        /// <summary>
        /// 添加许可令牌时间间隔
        /// </summary>
        public TimeSpan StableInterval { get; set; }

        /// <summary>
        /// 下次请求可以获取令牌的起始时间，默认当前系统时间戳（DateTime.UtcNow.Ticks）
        /// </summary>
        public long NextFreeTicketTimestamp { get; set; }

        /// <summary>
        /// 当前存储令牌数
        /// </summary>
        public decimal StoredPermits { get; set; }

        public TokenBucket()
        {

        }

        /// <summary>
        /// </summary>
        /// <param name="permitsPerSecond">每一秒允许的许可令牌</param>
        /// <param name="maxBurstSecond">最大存储maxBurstSeconds秒生成的令牌</param>
        public TokenBucket(decimal permitsPerSecond, decimal maxBurstSecond = 1)
        {
            PermitsPerSecond = permitsPerSecond;
            MaxBurstSecond = maxBurstSecond;

            //计算每一个令牌的生成，需要间隔多少秒
            var stableIntervalSeconds = (double)(1 / permitsPerSecond);
            var stableInterval = stableIntervalSeconds < TimeSpan.MaxValue.TotalSeconds
                ? TimeSpan.FromSeconds((double)(1 / permitsPerSecond)) : TimeSpan.MaxValue;
            this.StableInterval = stableInterval;

            //计算最大可以储存多少个令牌
            this.MaxPermits = (long)(permitsPerSecond * maxBurstSecond);
            this.StoredPermits = 0;

            //先使用当前时间的时间戳
            this.NextFreeTicketTimestamp = Stopwatch.GetTimestamp();
        }

        /// <summary>
        /// 获取令牌桶的失效时间(秒)
        /// </summary>
        /// <returns></returns>
        public long GetExpires()
        {
            //获取当前时间戳
            var nowTimestamp = Stopwatch.GetTimestamp();
            return 10 + (ParseDuration(Math.Max(NextFreeTicketTimestamp, nowTimestamp), nowTimestamp)).Seconds;
        }

        /// <summary>
        ///  异步更新当前持有的令牌数, 若当前时间晚于nextFreeTicketMicros，则计算该段时间内可以生成多少令牌
        ///  将生成的令牌加入令牌桶中并更新数据
        /// </summary>
        /// <param name="nowTimestamp">当前时间戳</param>
        public void ReSync(long nowTimestamp)
        {
            if (nowTimestamp > NextFreeTicketTimestamp)
            {
                var newDuration = ParseDuration(NextFreeTicketTimestamp, nowTimestamp);
                var newPermits = newDuration.Ticks / (decimal)StableInterval.Ticks;
                StoredPermits = Math.Min(MaxPermits, StoredPermits + newPermits);
                NextFreeTicketTimestamp = nowTimestamp;
            }
        }

        /// <summary>
        /// 计算2个时间的间隔
        /// </summary>
        /// <param name="from">起始时间（Ticks值, 如DateTime.Now.Ticks）</param>
        /// <param name="to">结束时间（Ticks值, 如DateTime.Now.Ticks）</param>
        /// <returns></returns>
        private TimeSpan ParseDuration(long from, long to)
        {
            return TimeSpan.FromSeconds((to - from) / (double)Stopwatch.Frequency);
        }
    }
}
