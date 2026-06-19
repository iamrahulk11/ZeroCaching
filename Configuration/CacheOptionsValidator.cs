using Microsoft.Extensions.Options;
using ZeroCaching.Configuration;

public sealed class CacheOptionsValidation : IValidateOptions<CacheOptions>
{
    public ValidateOptionsResult Validate(string? name, CacheOptions options)
    {
        if (options is null)
        {
            return ValidateOptionsResult.Fail("CacheOptions is null");
        }

        if (!options.Enabled)
        {
            return ValidateOptionsResult.Success;
        }

        if (!Enum.IsDefined(typeof(CacheProvider), options.Provider))
        {
            return ValidateOptionsResult.Fail($"Invalid CacheProvider: {options.Provider}");
        }

        if (options.Provider == CacheProvider.Redis &&
            string.IsNullOrWhiteSpace(options.ConnectionString))
        {
            return ValidateOptionsResult.Fail("Redis ConnectionString is required.");
        }

        if (options.DefaultExpirationMinutes <= 0)
        {
            options.DefaultExpirationMinutes = 30; // normalization allowed here
        }

        return ValidateOptionsResult.Success;
    }
}