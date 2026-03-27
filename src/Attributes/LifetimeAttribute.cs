namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Base attribute that describes how an attributed class should be registered in the dependency injection container.
/// </summary>
/// <remarks>
/// Apply this attribute, or one of its derived attributes, to a concrete class. Multiple lifetime attributes can be used
/// on the same implementation type to register it against different service types, lifetimes, or keys.
/// </remarks>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
public class LifetimeAttribute : Attribute
{
    /// <summary>
    /// Initializes a new instance of <see cref="LifetimeAttribute"/>.
    /// </summary>
    /// <param name="serviceLifetime">The lifetime to use when registering the attributed class.</param>
    /// <param name="services">
    /// The service types to register. When empty, the attributed class is registered as its own service type.
    /// </param>
    public LifetimeAttribute(ServiceLifetime serviceLifetime, params Type[] services)
    {
        ServiceLifetime = serviceLifetime;
        Services = services;
    }

#if NET8_0_OR_GREATER
    /// <summary>
    /// Initializes a new instance of <see cref="LifetimeAttribute"/> with a key.
    /// </summary>
    /// <param name="key">The key to associate with the keyed service registration.</param>
    /// <param name="serviceLifetime">The lifetime to use when registering the attributed class.</param>
    /// <param name="services">
    /// The service types to register. When empty, the attributed class is registered as its own service type.
    /// </param>
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
    /// Gets the lifetime that should be used for the registration.
    /// </summary>
    public ServiceLifetime ServiceLifetime { get; private set; }

    /// <summary>
    /// Gets the service types that the attributed class should be registered as.
    /// </summary>
    public Type[] Services { get; private set; }
}

#if NET8_0_OR_GREATER

/// <summary>
/// Base generic lifetime attribute for registering an implementation type as <typeparamref name="TService"/>.
/// </summary>
/// <typeparam name="TService">The service type to register.</typeparam>
public class LifetimeAttribute<TService> : LifetimeAttribute
{
    /// <summary>
    /// Initializes a new instance of <see cref="LifetimeAttribute{TService}"/>.
    /// </summary>
    /// <param name="serviceLifetime">The lifetime to use for the registration.</param>
    public LifetimeAttribute(ServiceLifetime serviceLifetime) : base(serviceLifetime, [typeof(TService)])
    {
    }

    /// <summary>
    /// Initializes a new instance of <see cref="LifetimeAttribute{TService}"/> with a key.
    /// </summary>
    /// <param name="key">The key to associate with the keyed service registration.</param>
    /// <param name="serviceLifetime">The lifetime to use for the registration.</param>
    public LifetimeAttribute(object? key, ServiceLifetime serviceLifetime) : base(key, serviceLifetime, [typeof(TService)])
    {
    }
}

/// <summary>
/// Base generic lifetime attribute for registering an implementation type as
/// <typeparamref name="TService1"/> and <typeparamref name="TService2"/>.
/// </summary>
/// <typeparam name="TService1">The first service type to register.</typeparam>
/// <typeparam name="TService2">The second service type to register.</typeparam>
public class LifetimeAttribute<TService1, TService2> : LifetimeAttribute
{
    /// <summary>
    /// Initializes a new instance of <see cref="LifetimeAttribute{TService1, TService2}"/>.
    /// </summary>
    /// <param name="serviceLifetime">The lifetime to use for the registration.</param>
    public LifetimeAttribute(ServiceLifetime serviceLifetime) : base(serviceLifetime, [typeof(TService1), typeof(TService2)])
    {
    }

    /// <summary>
    /// Initializes a new instance of <see cref="LifetimeAttribute{TService1, TService2}"/> with a key.
    /// </summary>
    /// <param name="key">The key to associate with the keyed service registration.</param>
    /// <param name="serviceLifetime">The lifetime to use for the registration.</param>
    public LifetimeAttribute(object? key, ServiceLifetime serviceLifetime) : base(key, serviceLifetime, [typeof(TService1), typeof(TService2)])
    {
    }
}

/// <summary>
/// Base generic lifetime attribute for registering an implementation type as
/// <typeparamref name="TService1"/>, <typeparamref name="TService2"/>, and <typeparamref name="TService3"/>.
/// </summary>
/// <typeparam name="TService1">The first service type to register.</typeparam>
/// <typeparam name="TService2">The second service type to register.</typeparam>
/// <typeparam name="TService3">The third service type to register.</typeparam>
public class LifetimeAttribute<TService1, TService2, TService3> : LifetimeAttribute
{
    /// <summary>
    /// Initializes a new instance of <see cref="LifetimeAttribute{TService1, TService2, TService3}"/>.
    /// </summary>
    /// <param name="serviceLifetime">The lifetime to use for the registration.</param>
    public LifetimeAttribute(ServiceLifetime serviceLifetime) : base(serviceLifetime, [typeof(TService1), typeof(TService2), typeof(TService3)])
    {
    }

    /// <summary>
    /// Initializes a new instance of <see cref="LifetimeAttribute{TService1, TService2, TService3}"/> with a key.
    /// </summary>
    /// <param name="key">The key to associate with the keyed service registration.</param>
    /// <param name="serviceLifetime">The lifetime to use for the registration.</param>
    public LifetimeAttribute(object? key, ServiceLifetime serviceLifetime) : base(key, serviceLifetime, [typeof(TService1), typeof(TService2), typeof(TService3)])
    {
    }
}

#endif
