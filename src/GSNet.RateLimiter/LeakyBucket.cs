using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GSNet.Common.Helper;

namespace GSNet.RateLimiter
{
    /// <summary>
    /// 表示一个漏桶
    /// 漏桶的每一滴水滴💧，代表一个请求
    /// </summary>
    [Serializable]
    public class LeakyBucket
    {
        /// <summary>
        /// 漏桶的容量
        /// </summary>
        public int Capacity { get; set; }

        /// <summary>
        /// 漏桶漏水的时间间隔
        /// </summary>
        public TimeSpan LeaksInterval { get; set; }

        /// <summary>
        /// 漏桶的水滴💧的数量
        /// </summary>
        public decimal Count { get; set; }

        /// <summary>
        /// 最后一滴水滴漏出的时间戳
        /// </summary>
        public long LastLeakTimestamp { get; set; }

        public LeakyBucket()
        {

        }

        /// <summary>
        /// </summary>
        public LeakyBucket(int capacity, TimeSpan leaksInterval)
        {
            Capacity = capacity;
            LeaksInterval = leaksInterval;

            //最后一滴水滴漏出的时间戳，初始化为0
            this.LastLeakTimestamp = 0;
        }

        /// <summary>
        ///  刷新同步 漏桶的信息
        /// </summary>
        /// <param name="nowTimestamp">当前时间戳</param>
        public void  ReSync(long nowTimestamp)
        {
            //如果当前时间 小于 最后一滴水滴留出的时间, 表示需要等待
            if (nowTimestamp <= LastLeakTimestamp)
            {
                //当前时间，距离最后一滴留出的时间，相距多长时间
                var newDuration = DateTimeHelper.ParseDuration(nowTimestamp, LastLeakTimestamp);
                //计算还没有漏出的水滴数量
                Count = newDuration.Ticks / (decimal)LeaksInterval.Ticks;
            }
            else
            { 
                Count = 0;
            } 
        }
    }
}
