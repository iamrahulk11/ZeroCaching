# ZeroCaching

ZeroCaching is a lightweight and configurable distributed caching library for .NET applications. It simplifies cache management by providing a single caching abstraction while allowing applications to enable, disable, or switch cache providers through configuration.

## Why ZeroCaching?

- Simple integration with existing .NET applications
- Configuration-driven setup
- Supports distributed caching with Redis
- Built-in fallback when caching is disabled
- Startup configuration validation
- Dependency Injection friendly

## Installation

```bash
dotnet add package ZeroCaching
```

## Configuration

Add the following section to your `appsettings.json`:

```json
{
  "Cache": {
    "Enabled": true,
    "Provider": "Redis",
    "ConnectionString": "your-redis-connection-string",
    "DefaultExpirationMinutes": 30
  }
}
```

### Configuration Options

| Setting | Description |
|----------|-------------|
| Enabled | Enables or disables caching. |
| Provider | Cache provider to use. |
| ConnectionString | Connection string for the selected provider. |
| DefaultExpirationMinutes | Default expiration time for cached entries. |

## Service Registration

```csharp
builder.Services.AddZeroCaching(builder.Configuration);
```

## Usage

### Store Data

```csharp
await cacheService.SetAsync("product:1", product);
```

### Retrieve Data

```csharp
var product = await cacheService.GetAsync<Product>("product:1");
```

### Check Existence

```csharp
var exists = await cacheService.ExistsAsync("product:1");
```

### Remove Data

```csharp
await cacheService.RemoveAsync("product:1");
```

## How It Works

1. Configure caching through application settings.
2. Register ZeroCaching during application startup.
3. Inject the cache service where needed.
4. Start caching without worrying about provider-specific implementation details.

## Supported Provider

| Provider | Status |
|----------|---------|
| Redis | Supported |

## Benefits

- Easy to configure
- Provider-agnostic usage
- Distributed caching support
- Environment-specific configuration
- Clean integration with .NET Dependency Injection

## License

MIT License
