using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GSNet.RateLimiter.Configurations;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;

namespace GSNet.RateLimiter.Tests
{
    public class ServiceCollectionExtensionsTest
    {
        private readonly ITestOutputHelper _output;

        public ServiceCollectionExtensionsTest(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void Test_Add_TokenBucket_RateLimiters()
        {
            //��������IOC����ServiceCollection
            var services = new ServiceCollection();

            //���������
            services.AddRateLimiters(option =>
            {
                option.AddTokenBucketRateLimiter(x => {
                    x.MaxBurstSecond = 3;
                    x.PermitsPerSecond = 2;
                }, "TestTokenBucketRateLimiter");
            });

            //�����ṩ����
            var provider = services.BuildServiceProvider();

            var limiterManager = provider.GetService(typeof(IRateLimiterManager)) as IRateLimiterManager;

            //��������������ӦΪ��
            Assert.NotNull(limiterManager);

            var rateLimiter = limiterManager.GetRateLimiter("TestTokenBucketRateLimiter");

            //��������ӦΪ��
            Assert.NotNull(rateLimiter);

            //��ȡ����������
            Assert.Throws<Exception>(() => { limiterManager.GetRateLimiter("TestTokenBucketRateLimiterNotExists"); });

            Assert.IsType<TokenBucketRateLimiter>(rateLimiter);

            var tokenBucketRateLimiter = rateLimiter as TokenBucketRateLimiter;

            Assert.NotNull(tokenBucketRateLimiter);
            Assert.Equal("TestTokenBucketRateLimiter", tokenBucketRateLimiter.LimiterName);
        }

        [Fact]
        public void Test_Add_LeakyBucket_RateLimiters()
        {
            //��������IOC����ServiceCollection
            var services = new ServiceCollection();

            //���������
            services.AddRateLimiters(option =>
            {
                option.AddLeakyBucketRateLimiter(x => {
                    x.Capacity = 30;
                    x.LeaksInterval = TimeSpan.FromMilliseconds(200);
                }, "TestLeakyBucketRateLimiter");
            });

            //�����ṩ����
            var provider = services.BuildServiceProvider();

            var limiterManager = provider.GetService(typeof(IRateLimiterManager)) as IRateLimiterManager;

            //��������������ӦΪ��
            Assert.NotNull(limiterManager);

            var rateLimiter = limiterManager.GetRateLimiter("TestLeakyBucketRateLimiter");

            //��������ӦΪ��
            Assert.NotNull(rateLimiter);

            //��ȡ����������
            Assert.Throws<Exception>(() => { limiterManager.GetRateLimiter("TestLeakyBucketRateLimiterNotExists"); });

            Assert.IsType<LeakyBucketRateLimiter>(rateLimiter);

            var leakyBucketRateLimiter = rateLimiter as LeakyBucketRateLimiter;

            Assert.NotNull(leakyBucketRateLimiter);
            Assert.Equal("TestLeakyBucketRateLimiter", leakyBucketRateLimiter.LimiterName);
        }

        [Fact]
        public void Test_Add_Multi_RateLimiters()
        {
            //��������IOC����ServiceCollection
            var services = new ServiceCollection();

            //���������
            services.AddRateLimiters(option =>
            {
                option.AddLeakyBucketRateLimiter(x => {
                    x.Capacity = 30;
                    x.LeaksInterval = TimeSpan.FromMilliseconds(200);
                }, "TestLeakyBucketRateLimiter");
                option.AddTokenBucketRateLimiter(x => {
                    x.MaxBurstSecond = 3;
                    x.PermitsPerSecond = 2;
                }, "TestTokenBucketRateLimiter");
                option.AddTokenBucketRateLimiter(x => {
                    x.MaxBurstSecond = 5;
                    x.PermitsPerSecond = 5;
                }, "TestTokenBucketRateLimiterTwo");
            });

            //�����ṩ����
            var provider = services.BuildServiceProvider();

            var limiterManager = provider.GetService(typeof(IRateLimiterManager)) as IRateLimiterManager;

            //��������������ӦΪ��
            Assert.NotNull(limiterManager);

            var rateLimiters = provider.GetService(typeof(IEnumerable<IRateLimiter>)) as IEnumerable<IRateLimiter>;

            Assert.NotNull(rateLimiters);
            Assert.True(rateLimiters.Count() == 3);

            var leakyBucketRateLimiter = limiterManager.GetRateLimiter("TestLeakyBucketRateLimiter");
            var tokenBucketRateLimiter = limiterManager.GetRateLimiter("TestTokenBucketRateLimiter");
            var tokenBucketRateLimiterTwo = limiterManager.GetRateLimiter("TestTokenBucketRateLimiterTwo");

            Assert.NotNull(leakyBucketRateLimiter);
            Assert.NotNull(tokenBucketRateLimiter);
            Assert.NotNull(tokenBucketRateLimiterTwo);

            //��ȡ����������
            Assert.Throws<Exception>(() => { limiterManager.GetRateLimiter("xxx"); });
        }
    }
}