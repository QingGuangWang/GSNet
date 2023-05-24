using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GSNet.RateLimiter;
using GSNet.RateLimiter.Configurations;
using Microsoft.Extensions.DependencyInjection.Extensions;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    public static class RateLimiterServiceCollectionExtensions
    {
        public static IServiceCollection AddRateLimiters(this IServiceCollection services, Action<RateLimiterOptions> setupAction)
        {
            var options = new RateLimiterOptions();
            setupAction(options);
            foreach (var serviceExtension in options.Extensions)
            {
                serviceExtension.AddServices(services);
            }
            services.AddSingleton(options);

            //注册IRateLimiterManager
            services.TryAddSingleton<IRateLimiterManager, RateLimiterManager>();

            return services;
        }
    }
}
