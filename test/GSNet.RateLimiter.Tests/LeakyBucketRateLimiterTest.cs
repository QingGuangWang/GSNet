using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace GSNet.RateLimiter.Tests
{
    /// <summary>
    /// 基于漏桶算法(匀速排队)的限流器 的测试
    /// </summary>
    public class LeakyBucketRateLimiterTest
    {
        private readonly ITestOutputHelper _output;

        public LeakyBucketRateLimiterTest(ITestOutputHelper output)
        {
            _output = output;
        }

        /// <summary>
        /// 测试同步
        /// </summary>
        [Fact]
        public void Test_LeakyBucket_RateLimiter_Sync()
        {
            //漏桶，最大累计5个，漏速 500毫秒一个
            var limiter = new LeakyBucketRateLimiter("TestRateLimiter", 5, TimeSpan.FromMilliseconds(500));

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            //串行执行
            for (int i = 0; i < 10; i++)
            {
                var result = limiter.AcquirePermit();

                if (result.Succeed)
                {
                    _output.WriteLine($"AcquirePermit Success:  {DateTime.Now:O}");
                }
            }

            stopwatch.Stop();

            //执行10次，间隔500，扣除首次是不需要等待的。应该大于4500毫秒
            Assert.True(stopwatch.ElapsedMilliseconds >= ((10 - 1) * 500));

            _output.WriteLine($"执行10次，共耗时：{stopwatch.ElapsedMilliseconds} 毫秒");
        }

        /// <summary>
        /// 测试异步
        /// </summary>
        [Fact]
        public void Test_LeakyBucket_RateLimiter_Async()
        {
            //漏桶，最大累计5个，漏速 500毫秒一个
            var limiter = new LeakyBucketRateLimiter("TestRateLimiter", 5, TimeSpan.FromMilliseconds(500));

            var taskList = new List<Task>();

            var successCount = 0;
            var failCount = 0;

            //创建10个任务
            for (int i = 0; i < 10; i++)
            {
                taskList.Add(Task.Run(() =>
                {
                    var nowTime = DateTime.Now;
                    var result = limiter.AcquirePermit();
                    if (result.Succeed)
                    {
                        successCount++;
                    }
                    else
                    {
                        failCount++;
                    }
                }));
            }

            Task.WaitAll(taskList.ToArray());

            //桶的容量5个，最多等待5个 + 第一个访问不需要等待的，共6个可执行，剩下4个因为桶溢出而不可执行
            Assert.Equal(6, successCount);
            Assert.Equal(4, failCount);

            _output.WriteLine($"执行10次，成功的：{successCount} 个， 失败的：{failCount} 个");
        }
    }
}
