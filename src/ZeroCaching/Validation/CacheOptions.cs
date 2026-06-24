using Microsoft.Extensions.Options;
using ZeroCaching.Configuration;

namespace ZeroCaching.Validation;

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

        if (!Enum.IsDefined(options.Provider))
        {
            return ValidateOptionsResult.Fail(
                $"Invalid CacheProvider: {options.Provider}");
        }

        if (options.Provider == CacheProvider.Redis &&
            string.IsNullOrWhiteSpace(options.ConnectionString))
        {
            return ValidateOptionsResult.Fail("Redis ConnectionString is required.");
        }

        if (options.DefaultExpirationMinutes < 1)
        {
            return ValidateOptionsResult.Fail(
                "DefaultExpirationMinutes must be greater than or equal to 1.");
        }

        return ValidateOptionsResult.Success;
    }
}