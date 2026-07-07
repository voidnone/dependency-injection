namespace VoidNone.DependencyInjection.Tests;

/// <summary>
/// Tests registration paths that must work across all supported target frameworks.
/// </summary>
[TestClass]
public class ServiceCollectionCompatibilityTest
{
    internal interface IService { }

    internal interface IBaseService { }

    internal interface IDerivedService : IBaseService { }

    [Singleton(typeof(IService))]
    internal class InterfaceService : IService { }

    [Transient(typeof(IDerivedService), typeof(IBaseService))]
    internal class HierarchyService : IDerivedService { }

    private static IServiceProvider CreateProvider()
    {
        return new ServiceCollection()
            .AddFromAttributes()
            .BuildServiceProvider();
    }

    [TestMethod]
    public void InterfaceRegistration_ResolvesImplementation()
    {
        var provider = CreateProvider();

        var service = provider.GetService<IService>();

        Assert.IsNotNull(service);
        Assert.IsInstanceOfType<InterfaceService>(service);
    }

    [TestMethod]
    public void InterfaceHierarchyRegistration_ResolvesEachDeclaredService()
    {
        var provider = CreateProvider();

        var derivedService = provider.GetService<IDerivedService>();
        var baseService = provider.GetService<IBaseService>();

        Assert.IsNotNull(derivedService);
        Assert.IsNotNull(baseService);
        Assert.IsInstanceOfType<HierarchyService>(derivedService);
        Assert.IsInstanceOfType<HierarchyService>(baseService);
    }
}
