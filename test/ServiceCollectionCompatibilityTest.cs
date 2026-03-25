namespace VoidNone.DependencyInjectionTest;

/// <summary>
/// Tests registration paths that must work across all supported target frameworks.
/// </summary>
[TestClass]
public class ServiceCollectionCompatibilityTest
{
    private interface IService { }

    private interface IBaseService { }

    private interface IDerivedService : IBaseService { }

    [Singleton(typeof(IService))]
    private class InterfaceService : IService { }

    [Transient(typeof(IDerivedService), typeof(IBaseService))]
    private class HierarchyService : IDerivedService { }

    private static IServiceProvider CreateProvider()
    {
        return new ServiceCollection()
            .AddFromAssemblies(typeof(ServiceCollectionCompatibilityTest).Assembly)
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
