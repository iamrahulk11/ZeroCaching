using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using ZeroCaching.Abstractions;
using ZeroCaching.Configuration;
using ZeroCaching.Implementation;
using Microsoft.Extensions.Options;

namespace ZeroCaching.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddZeroCaching(
    this IServiceCollection services,
    IConfiguration configuration)
    {
        services.AddOptions<CacheOptions>()
            .Bind(configuration.GetSection("Cache"))
            .ValidateOnStart();

        services.AddSingleton<IValidateOptions<CacheOptions>, CacheOptionsValidation>();

        // build options once
        var options = configuration.GetSection("Cache").Get<CacheOptions>() ?? new();

        if (!options.Enabled)
        {
            services.AddSingleton<ICacheService, NoCacheService>();
            return services;
        }

        switch (options.Provider)
        {
            case CacheProvider.Redis:
                {
                    try
                    {
                        if (string.IsNullOrWhiteSpace(options.ConnectionString))
                            throw new InvalidOperationException("Redis connection string is required.");

                        var mux = ConnectionMultiplexer.Connect(options.ConnectionString);

                        if (!mux.IsConnected)
                            throw new InvalidOperationException("Unable to connect to Redis.");

                        services.AddSingleton<IConnectionMultiplexer>(mux);
                        services.AddSingleton<ICacheService, RedisCacheService>();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Redis failed: {ex.Message}");
                        services.AddSingleton<ICacheService, NoCacheService>();
                    }

                    break;
                }

            default:
                throw new NotSupportedException(
                    $"Unsupported cache provider: {options.Provider}");
        }

        return services;
    }
}