using ZeroCaching.Abstractions;

namespace ZeroCaching.Implementation;

internal sealed class NoCacheService : ICacheService
{
    public Task<T?> GetAsync<T>(string key)
        => Task.FromResult<T?>(default);

    public Task SetAsync<T>(string key, T value, TimeSpan? expiration = null)
        => Task.CompletedTask;

    public Task RemoveAsync(string key)
        => Task.CompletedTask;

    public Task<bool> ExistsAsync(string key)
        => Task.FromResult(false);
}