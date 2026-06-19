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
                        // Log the failure somewhere (ILogger ideally)
                        Console.WriteLine($"Redis failed to build connecion, falling back to NoCacheService: {ex.Message}");
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