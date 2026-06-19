namespace ZeroCaching.Configuration;

public static class CacheOptionsValidator
{
    public static void Validate(CacheOptions options)
    {
        if (options is null)
            throw new ArgumentNullException(nameof(options));

        if (!options.Enabled)
            return;

        // Enum validation (important)
        if (!Enum.IsDefined(typeof(CacheProvider), options.Provider))
        {
            throw new InvalidOperationException(
                $"Invalid Cache Provider: {options.Provider}");
        }

        // Required field validation
        if (options.Provider == CacheProvider.Redis)
        {
            if (string.IsNullOrWhiteSpace(options.ConnectionString))
            {
                throw new InvalidOperationException(
                    "Cache: ConnectionString is required for Redis provider.");
            }
        }

        // Range validation (optional but good)
        if (options.DefaultExpirationMinutes <= 0)
        {
            options.DefaultExpirationMinutes = 30;
        }
    }
}