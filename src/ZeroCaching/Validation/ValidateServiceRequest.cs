using ZeroCaching.Internals;

namespace ZeroCaching.Validation;

internal static class ValidateServiceRequest
{
    public static void ValidateKey(string key)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            throw new ArgumentException(
                exceptionMessages.CACHE_KEY_EMPTY_NOT_ALLOWED,
                nameof(key));
        }
    }

    public static void ValidateExpiry(TimeSpan? expiry)
    {
        if (expiry.HasValue &&
            expiry.Value <= TimeSpan.Zero)
        {
            throw new ArgumentOutOfRangeException(
                nameof(expiry),
                expiry,
                exceptionMessages.DEFAULT_EXPIRATION_MINUTES_MUST_BE_GREATER_THAN_ZERO);
        }
    }
}