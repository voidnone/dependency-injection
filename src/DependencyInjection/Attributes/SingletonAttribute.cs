namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Marks the attributed class for singleton registration.
/// </summary>
public class SingletonAttribute : LifetimeAttribute
{
    /// <summary>
    /// Initializes a new instance of <see cref="SingletonAttribute"/>.
    /// </summary>
    /// <param name="services">
    /// The service types to register. When empty, the attributed class is registered as itself.
    /// </param>
    public SingletonAttribute(params Type[] services) : base(ServiceLifetime.Singleton, services)
    {
    }

#if NET8_0_OR_GREATER
    /// <summary>
    /// Initializes a new instance of <see cref="SingletonAttribute"/> with a key.
    /// </summary>
    /// <param name="key">The key to associate with the keyed singleton registration.</param>
    /// <param name="services">
    /// The service types to register. When empty, the attributed class is registered as itself.
    /// </param>
    public SingletonAttribute(object? key, params Type[] services) : base(key, ServiceLifetime.Singleton, services)
    {
    }
#endif
}

#if NET8_0_OR_GREATER

/// <summary>
/// Marks the attributed class for singleton registration as <typeparamref name="TService"/>.
/// </summary>
/// <typeparam name="TService">The service type to register.</typeparam>
public class SingletonAttribute<TService> : SingletonAttribute
{
    /// <summary>
    /// Initializes a new instance of <see cref="SingletonAttribute{TService}"/>.
    /// </summary>
    public SingletonAttribute() : base([typeof(TService)])
    {
    }

    /// <summary>
    /// Initializes a new instance of <see cref="SingletonAttribute{TService}"/> with a key.
    /// </summary>
    /// <param name="key">The key for keyed service registration.</param>
    public SingletonAttribute(object? key) : base(key, [typeof(TService)])
    {
    }
}

/// <summary>
/// Marks the attributed class for singleton registration as
/// <typeparamref name="TService1"/> and <typeparamref name="TService2"/>.
/// </summary>
/// <typeparam name="TService1">The first service type to register.</typeparam>
/// <typeparam name="TService2">The second service type to register.</typeparam>
public class SingletonAttribute<TService1, TService2> : SingletonAttribute
{
    /// <summary>
    /// Initializes a new instance of <see cref="SingletonAttribute{TService1, TService2}"/>.
    /// </summary>
    public SingletonAttribute() : base([typeof(TService1), typeof(TService2)])
    {
    }

    /// <summary>
    /// Initializes a new instance of <see cref="SingletonAttribute{TService1, TService2}"/> with a key.
    /// </summary>
    /// <param name="key">The key for keyed service registration.</param>
    public SingletonAttribute(object? key) : base(key, [typeof(TService1), typeof(TService2)])
    {
    }
}

/// <summary>
/// Marks the attributed class for singleton registration as
/// <typeparamref name="TService1"/>, <typeparamref name="TService2"/>, and <typeparamref name="TService3"/>.
/// </summary>
/// <typeparam name="TService1">The first service type to register.</typeparam>
/// <typeparam name="TService2">The second service type to register.</typeparam>
/// <typeparam name="TService3">The third service type to register.</typeparam>
public class SingletonAttribute<TService1, TService2, TService3> : SingletonAttribute
{
    /// <summary>
    /// Initializes a new instance of <see cref="SingletonAttribute{TService1, TService2, TService3}"/>.
    /// </summary>
    public SingletonAttribute() : base([typeof(TService1), typeof(TService2), typeof(TService3)])
    {
    }

    /// <summary>
    /// Initializes a new instance of <see cref="SingletonAttribute{TService1, TService2, TService3}"/> with a key.
    /// </summary>
    /// <param name="key">The key for keyed service registration.</param>
    public SingletonAttribute(object? key) : base(key, [typeof(TService1), typeof(TService2), typeof(TService3)])
    {
    }
}

#endif
