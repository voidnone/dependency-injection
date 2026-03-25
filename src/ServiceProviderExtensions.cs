using System.Collections.Concurrent;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Extension methods for <see cref="IServiceProvider"/> to retrieve registered services.
/// </summary>
public static class ServiceProviderExtensions
{
    private static readonly ConcurrentDictionary<Type, IEnumerable<Type>> serviceTypes = [];
    private static readonly ConcurrentDictionary<Type, object[]> serviceKeys = [];

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
        IEnumerable<Type> GetAllServiceTypes(Type type)
        {
            var serviceCollection = serviceProvider.GetRequiredService<IServiceCollection>();

            foreach (var item in serviceCollection)
            {
                if (item.ServiceType != serviceType) continue;
#if NET8_0_OR_GREATER
                if (item.IsKeyedService && item.KeyedImplementationType != null)
                {
                    yield return item.KeyedImplementationType;
                }
                else
#endif
                    if (item.ImplementationType != null)
                    {
                        yield return item.ImplementationType;
                    }
            }
        }

        return [.. serviceTypes.GetOrAdd(serviceType, GetAllServiceTypes)];
    }

#if NET8_0_OR_GREATER

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
        var keys = serviceKeys.GetOrAdd(serviceType, type =>
        {
            var serviceCollection = serviceProvider.GetRequiredService<IServiceCollection>();
            var keys = new HashSet<object>();

            foreach (var item in serviceCollection)
            {
                if (item.ServiceType != type) continue;
                if (item.IsKeyedService && item.ServiceKey != null)
                {
                    keys.Add(item.ServiceKey);
                }
            }
            return [.. keys];
        });

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