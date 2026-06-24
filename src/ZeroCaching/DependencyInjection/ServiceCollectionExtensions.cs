using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ZeroCaching.Abstractions;
using ZeroCaching.Configuration;
using ZeroCaching.Factories;
using ZeroCaching.Validation;

namespace ZeroCaching.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddZeroCaching(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddOptions<CacheOptions>()
            .Bind(configuration.GetSection(ConfigurationTag.Cache.ToString()))
            .ValidateOnStart();

        services.AddSingleton<IValidateOptions<CacheOptions>, CacheOptionsValidation>();

        services.AddSingleton<ICacheProviderFactory, CacheProviderFactory>();

        services.AddSingleton<ICacheService>(sp =>
            sp.GetRequiredService<ICacheProviderFactory>().Create());

        return services;
    }
}