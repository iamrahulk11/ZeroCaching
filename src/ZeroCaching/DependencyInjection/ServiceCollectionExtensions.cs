using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using ZeroCaching.Abstractions;
using ZeroCaching.Configuration;
using ZeroCaching.Factories;
using ZeroCaching.Services;
using ZeroCaching.Validation;

namespace ZeroCaching.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddZeroCaching(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // 1. Options + validation
        services.AddOptions<CacheOptions>()
            .Bind(configuration.GetSection("Cache"))
            .ValidateOnStart();

        services.AddSingleton<IValidateOptions<CacheOptions>, CacheOptionsValidation>();       

        // providers
        services.AddSingleton<RedisCacheService>();
        services.AddSingleton<NoCacheService>();

        // (decides actual runtime implementation)
        services.AddSingleton<ICacheProviderFactory, CacheProviderFactory>();

        // 5. Main abstraction exposed to consumers
        services.AddSingleton<ICacheService>(sp =>
            sp.GetRequiredService<ICacheProviderFactory>().Create());

        return services;
    }
}