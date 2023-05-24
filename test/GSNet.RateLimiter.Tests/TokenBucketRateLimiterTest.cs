using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace GSNet.RateLimiter.Tests
{
    public class TokenBucketRateLimiterTest
    {
        private readonly ITestOutputHelper _output;

        public TokenBucketRateLimiterTest(ITestOutputHelper output)
        {
            _output = output;
        }

        /// <summary>
        /// 测试同步
        /// </summary>
        [Fact]
        public void Test_TokenBucket_RateLimiter_Sync()
        {
            //令牌桶，每秒2个令牌，最大累计3秒产生的（6个）。
            var limiter = new TokenBucketRateLimiter("TestRateLimiter", 2, 3);

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            for (int i = 0; i < 10; i++)
            {
                var result = limiter.AcquirePermit();

                if (result.Succeed)
                {
                    _output.WriteLine($"AcquirePermit Success:  {DateTime.Now:O}");
                }
            }

            stopwatch.Stop();

            //执行10次，间隔500，扣除首次是不需要等待的（默认实现的令牌桶内部算法，当前请求的债由下一个请求来偿还）。应该大于4500毫秒
            Assert.True(stopwatch.ElapsedMilliseconds >= ((10 - 1) * 500));

            _output.WriteLine($"执行10次，共耗时：{stopwatch.ElapsedMilliseconds} 毫秒");
        }

        /// <summary>
        /// 测试同步, 突发流量
        /// </summary>
        [Fact]
        public void Test_TokenBucket_RateLimiter_Burst_Sync()
        {
            //令牌桶，每秒2个令牌（500毫秒一个），最大累计3秒产生的（6个）。
            var limiter = new TokenBucketRateLimiter("TestRateLimiter", 2, 3);

            //等待4秒，但是存储3秒生成的令牌，共6个。
            Thread.Sleep(4000);

            var stopwatch = new Stopwatch();

            stopwatch.Start();

            for (int i = 0; i < 10; i++)
            {
                var result = limiter.AcquirePermit();

                if (result.Succeed)
                {
                    _output.WriteLine($"AcquirePermit Success:  {DateTime.Now:O}");
                }
            }

            stopwatch.Stop();

            //执行10次，间隔500。
            //桶内有6个令牌，可以直接访问。 没有令牌的第一个不需要等待（默认实现的令牌桶内部算法，当前请求的债由下一个请求来偿还）
            //最后3个需要等待，应该大于等于1500毫秒。
            Assert.True(stopwatch.ElapsedMilliseconds >= ((10 - 6 - 1) * 500));

            _output.WriteLine($"执行10次，共耗时：{stopwatch.ElapsedMilliseconds} 毫秒");
        }

        [Fact]
        public void Test_TokenBucket_RateLimiter_Async()
        {
            //令牌桶，每秒2个令牌（500毫秒一个），最大累计3秒产生的（6个）。
            var limiter = new TokenBucketRateLimiter("TestRateLimiter", 2, 3);

            var taskList = new List<Task>();

            var successCount = 0;
            var failCount = 0;

            var stopwatch = new Stopwatch();

            stopwatch.Start();

            for (int i = 0; i < 10; i++)
            {
                taskList.Add(Task.Run(() =>
                {
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

            stopwatch.Stop();

            //令牌桶不像漏通，会有请求溢出的情况，所以失败的应该是0
            Assert.Equal(10, successCount);
            Assert.Equal(0, failCount);
            //执行10次，间隔500，扣除首次是不需要等待的（默认实现的令牌桶内部算法，当前请求的债由下一个请求来偿还）。应该大于4500毫秒， 考虑并发可能出现误差的情况下，给10毫秒误差范围
            Assert.True(stopwatch.ElapsedMilliseconds >= ((10 - 1) * 500) - 10);

            _output.WriteLine($"执行10次，成功的：{successCount} 个， 失败的：{failCount} 个， 共耗时：{stopwatch.ElapsedMilliseconds} 毫秒");
        }

        [Fact]
        public void Test_TokenBucket_RateLimiter_Timeout_Async()
        {
            //令牌桶，每秒2个令牌（500毫秒一个），最大累计3秒产生的（6个）。
            var limiter = new TokenBucketRateLimiter("TestRateLimiter", 2, 3);

            var taskList = new List<Task>();

            var successCount = 0;
            var failCount = 0;

            var stopwatch = new Stopwatch();

            stopwatch.Start();

            for (int i = 0; i < 10; i++)
            {
                taskList.Add(Task.Run(() =>
                {
                    var result = limiter.AcquirePermits(1, TimeSpan.FromSeconds(2));
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

            stopwatch.Stop();

            //等待超时是2秒，而令牌平均500毫秒一个，理论上只有4个请求可以，其余6个请求是失败的。 第一个请求不需要等待，第二个请求等待500，第三个，1000， 第四个1500；
            //第五个2000毫秒，但是结合其它操作的耗时叠加，会大于2000毫秒。
            Assert.Equal(4, successCount);
            Assert.Equal(6, failCount);
            //执行10次，间隔500，扣除首次是不需要等待的（默认实现的令牌桶内部算法，当前请求的债由下一个请求来偿还）。
            Assert.True(stopwatch.ElapsedMilliseconds >= ((4 - 1) * 500) - 10);

            _output.WriteLine($"执行10次，成功的：{successCount} 个， 失败的：{failCount} 个， 共耗时：{stopwatch.ElapsedMilliseconds} 毫秒");
        }
    }
}
