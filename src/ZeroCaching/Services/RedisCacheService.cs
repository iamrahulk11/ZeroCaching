using System.Text.Json;
using StackExchange.Redis;
using ZeroCaching.Abstractions;

namespace ZeroCaching.Services;

internal sealed class RedisCacheService : ICacheService
{
    private readonly IDatabase _database;

    public RedisCacheService(IConnectionMultiplexer connection)
    {
        _database = connection.GetDatabase();
    }

    public async Task<T?> GetAsync<T>(string key)
    {
        var value = await _database.StringGetAsync(key);

        if (value.IsNullOrEmpty)
            return default;

        return JsonSerializer.Deserialize<T>(value.ToString());
    }

    public Task SetAsync<T>(string key, T value, TimeSpan? ttl = null)
    {
        var json = JsonSerializer.Serialize(value);

        if (ttl.HasValue)
        {
            return _database.StringSetAsync(key, json, ttl.Value);
        }

        return _database.StringSetAsync(key, json);
    }

    public Task RemoveAsync(string key)
        => _database.KeyDeleteAsync(key);

    public Task<bool> ExistsAsync(string key)
        => _database.KeyExistsAsync(key);
}