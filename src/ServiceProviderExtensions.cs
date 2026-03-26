using System.Collections.Concurrent;
using System.Runtime.CompilerServices;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Extension methods for <see cref="IServiceProvider"/> to retrieve registered services.
/// </summary>
public static class ServiceProviderExtensions
{
    private static readonly ConditionalWeakTable<IServiceCollection, ConcurrentDictionary<Type, Type[]>> implementationTypesCache = [];

    /// <summary>
    /// Gets all implementation types registered for the specified service type.
    /// </summary>
    /// <typeparam name="T">The service type to look up.</typeparam>
    /// <param name="serviceProvider">The <see cref="IServiceProvider"/> to retrieve services from.</param>
    /// <returns>An array of implementation types registered for the service type.</returns>
    public static Type[] GetAllServiceTypes<T>(this IServiceProvider serviceProvider) => GetAllServiceTypes(serviceProvider, typeof(T));

    /// <summary>
    /// Gets all implementation types registered for the specified service type.
    /// </summary>
    /// <param name="serviceProvider">The <see cref="IServiceProvider"/> to retrieve services from.</param>
    /// <param name="serviceType">The service type to look up.</param>
    /// <returns>An array of implementation types registered for the service type.</returns>
    public static Type[] GetAllServiceTypes(this IServiceProvider serviceProvider, Type serviceType)
    {
        ArgumentNullException.ThrowIfNull(serviceProvider);
        ArgumentNullException.ThrowIfNull(serviceType);

        var serviceCollection = serviceProvider.GetRequiredService<IServiceCollection>();
        var cache = implementationTypesCache.GetOrCreateValue(serviceCollection);

        return cache.GetOrAdd(serviceType, static (key, services) =>
        [
            .. services
                .Where(item => item.ServiceType == key)
                .Select(GetImplementationType)
                .OfType<Type>()
                .Distinct()
        ], serviceCollection);

        static Type? GetImplementationType(ServiceDescriptor item)
        {
#if NET8_0_OR_GREATER
            if (item.IsKeyedService)
            {
                return item.KeyedImplementationType;
            }
#endif
            return item.ImplementationType ?? item.ImplementationInstance?.GetType();
        }
    }

#if NET8_0_OR_GREATER
    private static readonly ConditionalWeakTable<IServiceCollection, ConcurrentDictionary<Type, object?[]>> serviceKeysCache = [];

    /// <summary>
    /// Gets all service instances of type <typeparamref name="T"/>, including both regular and keyed services.
    /// </summary>
    /// <typeparam name="T">The service type to retrieve.</typeparam>
    /// <param name="serviceProvider">The <see cref="IServiceProvider"/> to retrieve services from.</param>
    /// <returns>An enumerable of all service instances of type <typeparamref name="T"/>.</returns>
    public static IEnumerable<T> GetAllServices<T>(this IServiceProvider serviceProvider) => GetAllServices(serviceProvider, typeof(T)).Select(s => (T)s);

    /// <summary>
    /// Gets all service instances for the specified service type, including both regular and keyed services.
    /// </summary>
    /// <param name="serviceProvider">The <see cref="IServiceProvider"/> to retrieve services from.</param>
    /// <param name="serviceType">The service type to retrieve.</param>
    /// <returns>An enumerable of all service instances for the specified service type.</returns>
    public static IEnumerable<object> GetAllServices(this IServiceProvider serviceProvider, Type serviceType)
    {
        ArgumentNullException.ThrowIfNull(serviceProvider);
        ArgumentNullException.ThrowIfNull(serviceType);

        var serviceCollection = serviceProvider.GetRequiredService<IServiceCollection>();
        var cache = serviceKeysCache.GetOrCreateValue(serviceCollection);
        var keys = cache.GetOrAdd(serviceType, static (key, services) =>
            [.. services
                .Where(item => item.ServiceType == key && item.IsKeyedService)
                .Select(item => item.ServiceKey)
                .Distinct()
            ], serviceCollection);

        foreach (var item in serviceProvider.GetServices(serviceType))
        {
            if (item != null) yield return item;
        }

        foreach (var key in keys)
        {
            foreach (var item in serviceProvider.GetKeyedServices(serviceType, key))
            {
                if (item != null) yield return item;
            }
        }
    }
#endif
}
