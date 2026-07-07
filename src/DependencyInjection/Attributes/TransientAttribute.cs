namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Marks the attributed class for transient registration.
/// </summary>
public class TransientAttribute : LifetimeAttribute
{
    /// <summary>
    /// Initializes a new instance of <see cref="TransientAttribute"/>.
    /// </summary>
    /// <param name="services">
    /// The service types to register. When empty, the attributed class is registered as itself.
    /// </param>
    public TransientAttribute(params Type[] services) : base(ServiceLifetime.Transient, services)
    {
    }

#if NET8_0_OR_GREATER
    /// <summary>
    /// Initializes a new instance of <see cref="TransientAttribute"/> with a key.
    /// </summary>
    /// <param name="key">The key to associate with the keyed transient registration.</param>
    /// <param name="services">
    /// The service types to register. When empty, the attributed class is registered as itself.
    /// </param>
    public TransientAttribute(object? key, params Type[] services) : base(key, ServiceLifetime.Transient, services)
    {
    }
#endif
}

#if NET8_0_OR_GREATER

/// <summary>
/// Marks the attributed class for transient registration as <typeparamref name="TService"/>.
/// </summary>
/// <typeparam name="TService">The service type to register.</typeparam>
public class TransientAttribute<TService> : TransientAttribute
{
    /// <summary>
    /// Initializes a new instance of <see cref="TransientAttribute{TService}"/>.
    /// </summary>
    public TransientAttribute() : base([typeof(TService)])
    {
    }

    /// <summary>
    /// Initializes a new instance of <see cref="TransientAttribute{TService}"/> with a key.
    /// </summary>
    /// <param name="key">The key for keyed service registration.</param>
    public TransientAttribute(object? key) : base(key, [typeof(TService)])
    {
    }
}

/// <summary>
/// Marks the attributed class for transient registration as
/// <typeparamref name="TService1"/> and <typeparamref name="TService2"/>.
/// </summary>
/// <typeparam name="TService1">The first service type to register.</typeparam>
/// <typeparam name="TService2">The second service type to register.</typeparam>
public class TransientAttribute<TService1, TService2> : TransientAttribute
{
    /// <summary>
    /// Initializes a new instance of <see cref="TransientAttribute{TService1, TService2}"/>.
    /// </summary>
    public TransientAttribute() : base([typeof(TService1), typeof(TService2)])
    {
    }

    /// <summary>
    /// Initializes a new instance of <see cref="TransientAttribute{TService1, TService2}"/> with a key.
    /// </summary>
    /// <param name="key">The key for keyed service registration.</param>
    public TransientAttribute(object? key) : base(key, [typeof(TService1), typeof(TService2)])
    {
    }
}

/// <summary>
/// Marks the attributed class for transient registration as
/// <typeparamref name="TService1"/>, <typeparamref name="TService2"/>, and <typeparamref name="TService3"/>.
/// </summary>
/// <typeparam name="TService1">The first service type to register.</typeparam>
/// <typeparam name="TService2">The second service type to register.</typeparam>
/// <typeparam name="TService3">The third service type to register.</typeparam>
public class TransientAttribute<TService1, TService2, TService3> : TransientAttribute
{
    /// <summary>
    /// Initializes a new instance of <see cref="TransientAttribute{TService1, TService2, TService3}"/>.
    /// </summary>
    public TransientAttribute() : base([typeof(TService1), typeof(TService2), typeof(TService3)])
    {
    }

    /// <summary>
    /// Initializes a new instance of <see cref="TransientAttribute{TService1, TService2, TService3}"/> with a key.
    /// </summary>
    /// <param name="key">The key for keyed service registration.</param>
    public TransientAttribute(object? key) : base(key, [typeof(TService1), typeof(TService2), typeof(TService3)])
    {
    }
}

#endif
