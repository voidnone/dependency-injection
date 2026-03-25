namespace VoidNone.DependencyInjectionTest;

/// <summary>
/// Tests for <see cref="ServiceProviderExtensions"/>.
/// </summary>
#if NET8_0_OR_GREATER
[TestClass]
public class ServiceProviderExtensionsTest
{
    private interface IService { }

    [Singleton<IService>]
    private class Service : IService { }

    [Singleton<IService>("key1")]
    private class KeyedService1 : IService { }

    [Singleton<IService>("key2")]
    private class KeyedService2 : IService { }

    private static IServiceProvider CreateProvider()
    {
        return new ServiceCollection()
            .AddFromAssemblies(typeof(ServiceProviderExtensionsTest).Assembly)
            .BuildServiceProvider();
    }

    [TestMethod]
    public void GetAllServiceTypes_ReturnsAllImplementations()
    {
        var provider = CreateProvider();

        var types = provider.GetAllServiceTypes<IService>();

        Assert.AreEqual(3, types.Length);
        Assert.IsTrue(types.Contains(typeof(Service)));
        Assert.IsTrue(types.Contains(typeof(KeyedService1)));
        Assert.IsTrue(types.Contains(typeof(KeyedService2)));
    }

    [TestMethod]
    public void GetAllServiceTypes_GenericOverload_Works()
    {
        var provider = CreateProvider();

        var types = provider.GetAllServiceTypes<IService>();

        Assert.AreEqual(3, types.Length);
    }

    [TestMethod]
    public void GetAllServices_ReturnsAllInstances()
    {
        var provider = CreateProvider();

        var instances = provider.GetAllServices<IService>().ToList();

        Assert.AreEqual(3, instances.Count);
        Assert.IsInstanceOfType<Service>(instances.First(i => i is Service));
        Assert.IsInstanceOfType<KeyedService1>(instances.First(i => i is KeyedService1));
        Assert.IsInstanceOfType<KeyedService2>(instances.First(i => i is KeyedService2));
    }

    [TestMethod]
    public void GetAllServices_GenericOverload_ReturnsTypedInstances()
    {
        var provider = CreateProvider();

        var instances = provider.GetAllServices<IService>();

        Assert.AreEqual(3, instances.Count());
    }

    [TestMethod]
    public void GetAllServices_SingletonInstances_AreSame()
    {
        var provider = CreateProvider();

        var instances1 = provider.GetAllServices<IService>().ToList();
        var instances2 = provider.GetAllServices<IService>().ToList();

        Assert.AreSame(instances1[0], instances2[0]);
        Assert.AreSame(instances1[1], instances2[1]);
        Assert.AreSame(instances1[2], instances2[2]);
    }
}
#endif
