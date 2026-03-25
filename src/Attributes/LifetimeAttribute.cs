namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Base attribute for marking a class with a specific service lifetime.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
public class LifetimeAttribute : Attribute
{
    /// <summary>
    /// Initializes a new instance of <see cref="LifetimeAttribute"/>.
    /// </summary>
    /// <param name="serviceLifetime">The <see cref="ServiceLifetime"/> of the service.</param>
    /// <param name="services">The service types to register.</param>
    public LifetimeAttribute(ServiceLifetime serviceLifetime, params Type[] services)
    {
        ServiceLifetime = serviceLifetime;
        Services = services;
    }

#if NET8_0_OR_GREATER
    /// <summary>
    /// Initializes a new instance of <see cref="LifetimeAttribute"/> with a key.
    /// </summary>
    /// <param name="key">The key for keyed service registration.</param>
    /// <param name="serviceLifetime">The <see cref="ServiceLifetime"/> of the service.</param>
    /// <param name="services">The service types to register.</param>
    public LifetimeAttribute(object? key, ServiceLifetime serviceLifetime, params Type[] services)
    {
        Key = key;
        ServiceLifetime = serviceLifetime;
        Services = services;
    }
#endif

    /// <summary>
    /// Gets the key used for keyed service registration.
    /// </summary>
    public object? Key { get; private set; }

    /// <summary>
    /// Gets the <see cref="ServiceLifetime"/> of the service.
    /// </summary>
    public ServiceLifetime ServiceLifetime { get; private set; }

    /// <summary>
    /// Gets the service types to register.
    /// </summary>
    public Type[] Services { get; private set; }
}

#if NET8_0_OR_GREATER

public class LifetimeAttribute<TService> : LifetimeAttribute
{
    public LifetimeAttribute(ServiceLifetime serviceLifetime) : base(serviceLifetime, [typeof(TService)])
    {
    }


    public LifetimeAttribute(object? key, ServiceLifetime serviceLifetime) : base(key, serviceLifetime, [typeof(TService)])
    {
    }
}

public class LifetimeAttribute<TService1, TService2> : LifetimeAttribute
{
    public LifetimeAttribute(ServiceLifetime serviceLifetime) : base(serviceLifetime, [typeof(TService1), typeof(TService2)])
    {
    }

    public LifetimeAttribute(object? key, ServiceLifetime serviceLifetime) : base(key, serviceLifetime, [typeof(TService1), typeof(TService2)])
    {
    }
}

public class LifetimeAttribute<TService1, TService2, TService3> : LifetimeAttribute
{
    public LifetimeAttribute(ServiceLifetime serviceLifetime) : base(serviceLifetime, [typeof(TService1), typeof(TService2), typeof(TService3)])
    {
    }

    public LifetimeAttribute(object? key, ServiceLifetime serviceLifetime) : base(key, serviceLifetime, [typeof(TService1), typeof(TService2), typeof(TService3)])
    {
    }
}

#endif