using Microsoft.Extensions.Options;
using StackExchange.Redis;
using ZeroCaching.Abstractions;
using ZeroCaching.Configuration;
using ZeroCaching.Services;

namespace ZeroCaching.Factories;

internal interface ICacheProviderFactory
{
    ICacheService Create();
}

internal sealed class CacheProviderFactory : ICacheProviderFactory
{
    private readonly CacheOptions _options;

    public CacheProviderFactory(IOptions<CacheOptions> options)
    {
        _options = options.Value;
    }

    public ICacheService Create()
    {
        var inner = CreateInnerCache();

        return new CacheValidationService(inner);
    }

    private ICacheService CreateInnerCache()
    {
        if (!_options.Enabled)
            return new NoCacheService();

        return _options.Provider switch
        {
            CacheProvider.Redis => CreateRedisOrFallback(),
            _ => new NoCacheService()
        };
    }

    private ICacheService CreateRedisOrFallback()
    {
        try
        {
            if (string.IsNullOrWhiteSpace(_options.ConnectionString))
                return new NoCacheService();

            var redis = ConnectionMultiplexer.Connect(_options.ConnectionString);

            return new RedisCacheService(redis);
        }
        catch
        {
            return new NoCacheService();
        }
    }
}