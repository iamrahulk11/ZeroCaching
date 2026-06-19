namespace ZeroCaching.Configuration;

public class CacheOptions
{
    public bool Enabled { get; set; }

    public CacheProvider Provider { get; set; }

    public string? ConnectionString { get; set; }

    public int DefaultExpirationMinutes { get; set; } = 30;
}