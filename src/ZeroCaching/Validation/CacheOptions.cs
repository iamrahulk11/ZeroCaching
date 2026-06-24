using Microsoft.Extensions.Options;
using ZeroCaching.Configuration;
using ZeroCaching.Internals;

namespace ZeroCaching.Validation;

public sealed class CacheOptionsValidation : IValidateOptions<CacheOptions>
{
    public ValidateOptionsResult Validate(string? name, CacheOptions options)
    {
        if (options is null)
        {
            return ValidateOptionsResult.Fail(exceptionMessages.CACHE_OPTION_NULL);
        }

        if (!options.Enabled)
        {
            return ValidateOptionsResult.Success;
        }

        if (!Enum.IsDefined(options.Provider))
        {
            return ValidateOptionsResult.Fail(
                $"{exceptionMessages.INVALID_CACHE_PROVIDER}: {options.Provider}");
        }

        if (options.Provider == CacheProvider.Redis &&
            string.IsNullOrWhiteSpace(options.ConnectionString))
        {
            return ValidateOptionsResult.Fail(exceptionMessages.REDIS_CONNECTION_REQUIRED);
        }

        if (options.DefaultExpirationMinutes < 1)
        {
            return ValidateOptionsResult.Fail(
                exceptionMessages.DEFAULT_EXPIRATION_MINUTES_MUST_BE_GREATER_THAN_ZERO);
        }

        return ValidateOptionsResult.Success;
    }
}