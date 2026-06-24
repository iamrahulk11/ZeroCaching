# ZeroCaching

ZeroCaching is a lightweight and configurable distributed caching library for .NET applications. 
It simplifies cache management by providing a single caching abstraction while allowing applications to enable, disable, or switch cache providers through configuration 
and graceful degradation when caching is unavailable.

---

## Project Structure

```
ZeroCaching.sln
│
├── src
│   └── ZeroCaching
│       ├── ZeroCaching.csproj
│       │
│       ├── Abstractions
│       ├── Configuration
│       ├── Factories
│       ├── Services
│       ├── Validation
│       ├── Internals
│       └── DependencyInjection
│
└── README.md
```

---

## Why ZeroCaching?

- Simple integration with existing .NET applications  
- Configuration-driven setup  
- Supports distributed caching  
- Built-in fallback when caching is disabled  
- Startup configuration validation  
- Dependency Injection friendly  

---

## Installation

```bash
dotnet add package ZeroCaching
```

---

## Configuration

ZeroCaching is configured using the `appsettings.json` file.  
This allows you to enable or disable caching and switch providers without changing code.

### appsettings.json

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

---

## Configuration Options

| Setting | Type | Description |
|--------|------|-------------|
| Enabled | bool | Enables or disables caching globally |
| Provider | string | Cache provider to use (e.g., Redis) |
| ConnectionString | string | Connection string for the selected provider |
| DefaultExpirationMinutes | int | Default expiration time (in minutes) for cached entries |

---

## Example Configuration Scenarios

### Disable Caching

```json
{
  "Cache": {
    "Enabled": false
  }
}
```

### Redis Configuration

```json
{
  "Cache": {
    "Enabled": true,
    "Provider": "Redis",
    "ConnectionString": "localhost:6379",
    "DefaultExpirationMinutes": 60
  }
}
```

---

## Service Registration

```csharp
builder.Services.AddZeroCaching(builder.Configuration);
```

---

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

---

## How It Works

- Configure caching through `appsettings.json`
- Register ZeroCaching in application startup
- Inject cache service where needed
- Use caching without worrying about provider-specific logic

---

## Supported Providers

| Provider | Status |
|----------|--------|
| Redis    | Supported |

---

## Benefits

- Easy configuration  
- Provider-agnostic usage  
- Distributed caching support  
- Environment-based configuration  
- Clean .NET Dependency Injection integration  