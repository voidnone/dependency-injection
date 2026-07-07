namespace VoidNone.DependencyInjectionTest;

/// <summary>
/// Tests for service registration with interface inheritance.
/// </summary>
[TestClass]
public class InheritanceTest
{
    [Transient(typeof(IBaseService))]
    internal class Service : BaseService, IService { }

    [Transient]
    internal class BaseService { }

    internal interface IService : IBaseService { }
    internal interface IBaseService { }

    private static IServiceProvider CreateProvider()
    {
        return new ServiceCollection()
            .AddFromAttributes()
            .BuildServiceProvider();
    }

    [TestMethod]
    public void BaseClass_CanBeRetrieved()
    {
        var provider = CreateProvider();

        var service = provider.GetService<Service>();
        var baseService = provider.GetService<BaseService>();

        Assert.IsNull(service);
        Assert.IsNotNull(baseService);
    }

    [TestMethod]
    public void DerivedInterface_CanBeRetrieved()
    {
        var provider = CreateProvider();

        var service = provider.GetService<IService>();

        Assert.IsNull(service);
    }

    [TestMethod]
    public void BaseInterface_CanBeRetrieved()
    {
        var provider = CreateProvider();

        var baseService = provider.GetService<IBaseService>();

        Assert.IsNotNull(baseService);
        Assert.IsInstanceOfType(baseService, typeof(Service));
    }
}
