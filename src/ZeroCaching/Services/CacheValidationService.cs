using ZeroCaching.Abstractions;
using ZeroCaching.Validation;

namespace ZeroCaching.Services;

internal sealed class CacheValidationService : ICacheService
{
    private readonly ICacheService _inner;

    public CacheValidationService(ICacheService inner)
    {
        _inner = inner;
    }

    public async Task<T?> GetAsync<T>(string key)
    {
        ValidateServiceRequest.ValidateKey(key);

        return await _inner.GetAsync<T>(key);
    }

    public async Task SetAsync<T>(
        string key,
        T value,
        TimeSpan? expiry = null)
    {
        ValidateServiceRequest.ValidateKey(key);
        ValidateServiceRequest.ValidateExpiry(expiry);

        await _inner.SetAsync(key, value, expiry);
    }

    public async Task RemoveAsync(string key)
    {
        ValidateServiceRequest.ValidateKey(key);

        await _inner.RemoveAsync(key);
    }

    public async Task<bool> ExistsAsync(string key)
    {
        ValidateServiceRequest.ValidateKey(key);

        return await _inner.ExistsAsync(key);
    }
}