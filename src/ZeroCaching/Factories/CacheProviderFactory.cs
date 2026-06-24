using Microsoft.Extensions.DependencyInjection;
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
    private readonly IServiceProvider _sp;

    public CacheProviderFactory(
        IOptions<CacheOptions> options,
        IServiceProvider sp)
    {
        _options = options.Value;
        _sp = sp;
    }

    public ICacheService Create()
    {
        if (!_options.Enabled)
            return _sp.GetRequiredService<NoCacheService>();

        if (_options.Provider == CacheProvider.Redis)
        {
            try
            {
                var redis = ConnectionMultiplexer.Connect(_options.ConnectionString!);
                return new RedisCacheService(redis);
            }
            catch
            {
                // fallback
                return _sp.GetRequiredService<NoCacheService>();
            }
        }

        return _sp.GetRequiredService<NoCacheService>();
    }
}