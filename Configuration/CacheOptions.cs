namespace ZeroCaching.Configuration;

public class CacheOptions
{
    public bool Enabled { get; set; }

    public CacheProvider Provider { get; set; }

    public string? ConnectionString { get; set; }

    public int DefaultExpirationMinutes { get; set; } = 30;
}
public void Validate()
    {
        if (!Enabled)
            return;

        if (Provider == CacheProvider.Redis &&
            string.IsNullOrWhiteSpace(ConnectionString))
        {
            throw new InvalidOperationException(
                "Redis ConnectionString is required.");
        }
    }