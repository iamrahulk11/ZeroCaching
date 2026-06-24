namespace ZeroCaching.Validation;

internal static class ValidateServiceRequest
{
    public static void ValidateKey(string key)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            throw new ArgumentException(
                "Cache key cannot be null or empty.",
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
                "Expiration must be greater than zero.");
        }
    }
}