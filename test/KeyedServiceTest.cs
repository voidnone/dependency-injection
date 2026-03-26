namespace VoidNone.DependencyInjectionTest;

#if NET8_0_OR_GREATER
/// <summary>
/// Tests for keyed service registration.
/// </summary>
[TestClass]
public class KeyedServiceTest
{
    [Singleton("key1")]
    private class KeyedSingletonService { }

    [Scoped("key2")]
    private class KeyedScopedService { }

    [Transient("key3")]
    private class KeyedTransientService { }

    [TestMethod]
    public void KeyedSingleton_CreatesSameInstance()
    {
        var provider = CreateProvider();

        var service1 = provider.GetKeyedService<KeyedSingletonService>("key1");
        var service2 = provider.GetKeyedService<KeyedSingletonService>("key1");

        Assert.IsNotNull(service1);
        Assert.IsNotNull(service2);
        Assert.AreSame(service1, service2);
    }

    [TestMethod]
    public void KeyedTransient_CreatesNewInstanceEachTime()
    {
        var provider = CreateProvider();

        var service1 = provider.GetKeyedService<KeyedTransientService>("key3");
        var service2 = provider.GetKeyedService<KeyedTransientService>("key3");

        Assert.IsNotNull(service1);
        Assert.IsNotNull(service2);
        Assert.AreNotSame(service1, service2);
    }

    [TestMethod]
    public void KeyedScoped_SameWithinScope()
    {
        var provider = CreateProvider();

        using var scope = provider.CreateScope();
        var service1 = scope.ServiceProvider.GetKeyedService<KeyedScopedService>("key2");
        var service2 = scope.ServiceProvider.GetKeyedService<KeyedScopedService>("key2");

        Assert.IsNotNull(service1);
        Assert.IsNotNull(service2);
        Assert.AreSame(service1, service2);
    }

    [TestMethod]
    public void GetService_ReturnsNull_ForKeyedService()
    {
        var provider = CreateProvider();

        var service = provider.GetService<KeyedSingletonService>();

        Assert.IsNull(service);
    }

    [TestMethod]
    public void GetKeyedService_ReturnsNull_ForWrongKey()
    {
        var provider = CreateProvider();

        var service = provider.GetKeyedService<KeyedSingletonService>("wrong_key");

        Assert.IsNull(service);
    }

    private static IServiceProvider CreateProvider()
    {
        return new ServiceCollection()
            .AddFromAssemblies(typeof(KeyedServiceTest).Assembly)
            .BuildServiceProvider();
    }
}
#endif
