namespace ZeroCaching.Internals;

public record exceptionMessages
{
    public const string REDIS_CONNECTION_REQUIRED = "Redis ConnectionString is required";
    public const string DEFAULT_EXPIRATION_MINUTES_MUST_BE_GREATER_THAN_ZERO = "DefaultExpirationMinutes must be greater than or equal to 1";
    public const string INVALID_CACHE_PROVIDER = "Invalid Cache Provider";
    public const string CACHE_OPTION_NULL = "CacheOptions is null";
    public const string CACHE_KEY_EMPTY_NOT_ALLOWED = "Cache key cannot be null or empty";
}