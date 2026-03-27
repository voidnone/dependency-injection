using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Provides attribute-based registration extensions for <see cref="IServiceCollection"/>.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Scans the specified assemblies and registers every non-abstract class decorated with
    /// one or more <see cref="LifetimeAttribute"/> instances.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add services to.</param>
    /// <param name="assemblies">The assemblies to scan for attributed implementation types.</param>
    /// <returns>The same <see cref="IServiceCollection"/> instance so calls can be chained.</returns>
    /// <remarks>
    /// <para>
    /// When no explicit service types are supplied by an attribute, the implementation type is registered as self.
    /// </para>
    /// <para>
    /// This method also registers the current <see cref="IServiceCollection"/> instance if it is not already present,
    /// which allows <see cref="ServiceProviderExtensions"/> to inspect the final registration set later.
    /// </para>
    /// </remarks>
    public static IServiceCollection AddFromAssemblies(this IServiceCollection services, params Assembly[] assemblies)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(assemblies);

        var types = assemblies.SelectMany(GetLoadableTypes).Distinct().ToArray();

        foreach (var type in types)
        {
            if (type == null || type.IsAbstract || type.IsInterface) continue;
            AddFromType(services, type);
        }

        if (services.All(a => a.ServiceType != typeof(IServiceCollection)))
        {
            services.AddSingleton(services);
        }

        return services;
    }

    private static void AddFromType(IServiceCollection services, Type type)
    {
        var attributes = type.GetCustomAttributes<LifetimeAttribute>().ToArray();
        if (attributes.Length == 0) return;

        var grouped = attributes.GroupBy(g => new { g.ServiceLifetime, g.Key });

        foreach (var group in grouped)
        {
            var serviceTypes = group.SelectMany(s => s.Services).Distinct().ToArray();

            ValidateServiceTypes(type, serviceTypes);
            // Register more specific service types first so the descriptor order stays deterministic.
            Array.Sort(serviceTypes, CompareServiceTypes);

            AddServices(services, type, group.Key.ServiceLifetime, group.Key.Key, serviceTypes);
        }
    }

    private static void AddServices(IServiceCollection services, Type implementationType, ServiceLifetime lifetime, object? key, Type[] serviceTypes)
    {
        if (serviceTypes.Length == 0)
        {
#if NET8_0_OR_GREATER
            services.Add(new ServiceDescriptor(implementationType, key, implementationType, lifetime));
#else
            services.Add(new ServiceDescriptor(implementationType, implementationType, lifetime));
#endif
        }
        else
        {
            foreach (var typeService in serviceTypes)
            {
#if NET8_0_OR_GREATER
                services.Add(new ServiceDescriptor(typeService, key, implementationType, lifetime));
#else
                services.Add(new ServiceDescriptor(typeService, implementationType, lifetime));
#endif
            }
        }
    }

    private static IEnumerable<Type> GetLoadableTypes(Assembly assembly)
    {
        try
        {
            return assembly.GetTypes();
        }
        catch (ReflectionTypeLoadException exception)
        {
            // Keep partial discovery working when some types fail to load.
            return exception.Types.OfType<Type>();
        }
    }

    private static void ValidateServiceTypes(Type implementationType, Type[] serviceTypes)
    {
        foreach (var serviceType in serviceTypes)
        {
            if (CanRegisterAsService(implementationType, serviceType))
            {
                continue;
            }

            throw new InvalidOperationException($"Type '{implementationType}' cannot be registered as service '{serviceType}'.");
        }
    }

    private static bool CanRegisterAsService(Type implementationType, Type serviceType)
    {
        if (serviceType.IsAssignableFrom(implementationType))
        {
            return true;
        }

        if (!serviceType.IsGenericTypeDefinition)
        {
            return false;
        }

        return GetCandidateServiceTypes(implementationType)
            .Any(candidate => candidate.IsGenericType && candidate.GetGenericTypeDefinition() == serviceType);
    }

    private static IEnumerable<Type> GetCandidateServiceTypes(Type implementationType)
    {
        yield return implementationType;

        foreach (var interfaceType in implementationType.GetInterfaces())
        {
            yield return interfaceType;
        }

        var current = implementationType.BaseType;
        while (current != null)
        {
            yield return current;
            current = current.BaseType;
        }
    }

    private static int CompareServiceTypes(Type? left, Type? right)
    {
        if (ReferenceEquals(left, right))
        {
            return 0;
        }

        if (left is null)
        {
            return 1;
        }

        if (right is null)
        {
            return -1;
        }

        if (left.IsAssignableFrom(right))
        {
            return 1;
        }

        if (right.IsAssignableFrom(left))
        {
            return -1;
        }

        return StringComparer.Ordinal.Compare(left.FullName, right.FullName);
    }
}
