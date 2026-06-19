using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using ZeroCaching.Abstractions;
using ZeroCaching.Configuration;
using ZeroCaching.Implementation;

namespace ZeroCaching.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddZeroCaching(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var options = configuration
                .GetSection("Cache")
                .Get<CacheOptions>() ?? new();

        if (!options.Enabled)
        {
            services.AddSingleton<ICacheService, NoCacheService>();
            return services;
        }

        switch (options.Provider)
        {
            case CacheProvider.Redis:
                services.AddSingleton<IConnectionMultiplexer>(_ =>
                    ConnectionMultiplexer.Connect(options.ConnectionString!));

                services.AddSingleton<ICacheService, RedisCacheService>();
                break;

            default:
                throw new NotSupportedException(
                    $"Unsupported cache provider: {options.Provider}");
        }

        return services;
    }
}