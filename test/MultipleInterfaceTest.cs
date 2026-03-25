namespace VoidNone.DependencyInjectionTest;

/// <summary>
/// Tests for registering a service with multiple interfaces.
/// </summary>
#if NET8_0_OR_GREATER
[TestClass]
public class MultipleInterfaceTest
{
    [Transient(typeof(IService), typeof(IService2))]
    private class Service : IService, IService2 { }

    [Singleton<IService3>, Transient<IService4, IService5, IService6>]
    private class MultiAttributeService : IService3, IService4, IService5, IService6 { }

    private interface IService { }
    private interface IService2 { }
    private interface IService3 { }
    private interface IService4 { }
    private interface IService5 { }
    private interface IService6 { }

    private static IServiceProvider CreateProvider()
    {
        return new ServiceCollection()
            .AddFromAssemblies(typeof(MultipleInterfaceTest).Assembly)
            .BuildServiceProvider();
    }

    [TestMethod]
    public void SingleAttribute_MultipleInterfaces()
    {
        var provider = CreateProvider();

        var service = provider.GetService<IService>();
        var service2 = provider.GetService<IService2>();

        Assert.IsNotNull(service);
        Assert.IsNotNull(service2);
        Assert.IsInstanceOfType(service, typeof(Service));
        Assert.IsInstanceOfType(service2, typeof(Service));
    }

    [TestMethod]
    public void MultipleAttributes_MultipleInterfaces()
    {
        var provider = CreateProvider();

        var service3 = provider.GetService<IService3>();
        var service4 = provider.GetService<IService4>();
        var service5 = provider.GetService<IService5>();
        var service6 = provider.GetService<IService6>();

        Assert.IsNotNull(service3);
        Assert.IsNotNull(service4);
        Assert.IsNotNull(service5);
        Assert.IsNotNull(service6);
        Assert.IsInstanceOfType(service3, typeof(MultiAttributeService));
        Assert.IsInstanceOfType(service4, typeof(MultiAttributeService));
        Assert.IsInstanceOfType(service5, typeof(MultiAttributeService));
        Assert.IsInstanceOfType(service6, typeof(MultiAttributeService));
    }

    [TestMethod]
    public void ServiceInstance_CannotBeRetrievedByClass()
    {
        var provider = CreateProvider();

        var service = provider.GetService<Service>();

        Assert.IsNull(service);
    }
}
#endif
