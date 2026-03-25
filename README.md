# VoidNone.DependencyInjection

A lightweight .NET dependency injection extension library that supports automatic service registration via attributes.

## Features

- **Attribute-based registration**: Mark classes with `[Singleton]`, `[Scoped]`, or `[Transient]` attributes
- **Interface support**: Register services with one or multiple interface types
- **Keyed services**: Support for keyed service registration (requires .NET 8+)
- **Generic attributes**: Convenient generic attribute syntax (requires .NET 8+)

## Installation

[![Nuget](https://img.shields.io/nuget/v/VoidNone.DependencyInjection?label=nuget&style=for-the-badge)](https://www.nuget.org/packages/VoidNone.DependencyInjection/)

```bash
dotnet add package VoidNone.DependencyInjection
```

## Quick Start

### 1. Mark your services

```csharp
[Singleton]
class DatabaseService { }

[Scoped]
class UserService { }

[Transient]
class LoggerService { }
```

### 2. Register services

```csharp
services.AddFromAssemblies(typeof(Service1).Assembly);
```

## Usage

### Service Lifetime Attributes

| Attribute | Description |
|-----------|-------------|
| `[Singleton]` | Single instance for the lifetime of the application |
| `[Scoped]` | One instance per request/scope |
| `[Transient]` | New instance each time it's requested |

### Registering with Interfaces

```csharp
interface IService { }
interface IBaseService { }

[Singleton<IService>]
class MyService : IService { }

// Register with multiple interfaces
[Singleton<IService, IBaseService>]
class MyService2 : IService, IBaseService { }
```

### Keyed Services (.NET 8+)

```csharp
[Singleton<IService>("primary")]
class PrimaryService : IService { }

[Singleton<IService>("backup")]
class BackupService : IService { }
```

### Extended Service Retrieval

```csharp
var provider = services
    .AddFromAssemblies(typeof(MyService).Assembly)
    .BuildServiceProvider();

// Get all implementation types for a service
Type[] types = provider.GetAllServiceTypes<IService>();

// Get all service instances (including keyed services) (.NET 8+)
IEnumerable<IService> instances = provider.GetAllServices<IService>();
```

## License

MIT
