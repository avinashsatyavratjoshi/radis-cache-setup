using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RadisCache.Radis.Configuration;
using RadisCache.Radis.ConnectionWrapper;
using RedisCache.Radis.RedisCacheOperations;

namespace RadisCache.Radis
{
    public static class RedisCacheConnector
    {
        public static IServiceCollection ConfigureRadisCache(this IServiceCollection services, IConfigurationSection configurationSection)
        {
            services.AddSingleton<IRedisCacheConfiguration, RedisCacheConfiguration>();
            services.AddSingleton<IRedisCacheConnectionWrapper, RedisCacheConnectionWrapper>();
            services.AddSingleton<IRedisCacheOperationsService, RedisCacheOperationsService>();

            services.AddSingleton(configurationSection.Get<RedisCacheConfiguration>());
            
            return services;
        }
    }
}
