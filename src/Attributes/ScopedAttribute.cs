namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Marks a class as a scoped service.
/// </summary>
public class ScopedAttribute : LifetimeAttribute
{
    /// <summary>
    /// Initializes a new instance of <see cref="ScopedAttribute"/>.
    /// </summary>
    /// <param name="services">The service types to register.</param>
    public ScopedAttribute(params Type[] services) : base(ServiceLifetime.Scoped, services)
    {

    }

#if NET8_0_OR_GREATER
    /// <summary>
    /// Initializes a new instance of <see cref="ScopedAttribute"/> with a key.
    /// </summary>
    /// <param name="key">The key for keyed service registration.</param>
    /// <param name="services">The service types to register.</param>
    public ScopedAttribute(object? key, params Type[] services) : base(key, ServiceLifetime.Scoped, services)
    {

    }
#endif

}

#if NET8_0_OR_GREATER

/// <summary>
/// Marks a class as a scoped service for <typeparamref name="TService"/>.
/// </summary>
/// <typeparam name="TService">The service type to register.</typeparam>
public class ScopedAttribute<TService> : ScopedAttribute
{
    /// <summary>
    /// Initializes a new instance of <see cref="ScopedAttribute{TService}"/>.
    /// </summary>
    public ScopedAttribute() : base([typeof(TService)])
    {

    }

    /// <summary>
    /// Initializes a new instance of <see cref="ScopedAttribute{TService}"/> with a key.
    /// </summary>
    /// <param name="key">The key for keyed service registration.</param>
    public ScopedAttribute(object? key) : base(key, [typeof(TService)])
    {

    }
}

/// <summary>
/// Marks a class as a scoped service for <typeparamref name="TService1"/> and <typeparamref name="TService2"/>.
/// </summary>
/// <typeparam name="TService1">The first service type to register.</typeparam>
/// <typeparam name="TService2">The second service type to register.</typeparam>
public class ScopedAttribute<TService1, TService2> : ScopedAttribute
{
    /// <summary>
    /// Initializes a new instance of <see cref="ScopedAttribute{TService1, TService2}"/>.
    /// </summary>
    public ScopedAttribute() : base([typeof(TService1), typeof(TService2)])
    {

    }

    /// <summary>
    /// Initializes a new instance of <see cref="ScopedAttribute{TService1, TService2}"/> with a key.
    /// </summary>
    /// <param name="key">The key for keyed service registration.</param>
    public ScopedAttribute(object? key) : base(key, [typeof(TService1), typeof(TService2)])
    {

    }
}

/// <summary>
/// Marks a class as a scoped service for <typeparamref name="TService1"/>, <typeparamref name="TService2"/>, and <typeparamref name="TService3"/>.
/// </summary>
/// <typeparam name="TService1">The first service type to register.</typeparam>
/// <typeparam name="TService2">The second service type to register.</typeparam>
/// <typeparam name="TService3">The third service type to register.</typeparam>
public class ScopedAttribute<TService1, TService2, TService3> : ScopedAttribute
{
    /// <summary>
    /// Initializes a new instance of <see cref="ScopedAttribute{TService1, TService2, TService3}"/>.
    /// </summary>
    public ScopedAttribute() : base([typeof(TService1), typeof(TService2), typeof(TService3)])
    {

    }

    /// <summary>
    /// Initializes a new instance of <see cref="ScopedAttribute{TService1, TService2, TService3}"/> with a key.
    /// </summary>
    /// <param name="key">The key for keyed service registration.</param>
    public ScopedAttribute(object? key) : base(key, [typeof(TService1), typeof(TService2), typeof(TService3)])
    {

    }
}

#endif
