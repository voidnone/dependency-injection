namespace VoidNone.DependencyInjectionTest;

#if NET8_0_OR_GREATER
/// <summary>
/// Tests that service provider inspection does not leak cached metadata across containers.
/// </summary>
[TestClass]
public class ServiceProviderIsolationTest
{
    private interface IService { }

    private class ProviderOneService : IService { }

    private class ProviderTwoService : IService { }

    private static IServiceProvider CreateProvider<TImplementation>()
        where TImplementation : class, IService
    {
        var services = new ServiceCollection();
        services.AddSingleton<IService, TImplementation>();
        services.AddSingleton<IServiceCollection>(services);
        return services.BuildServiceProvider();
    }

    [TestMethod]
    public void GetAllServiceTypes_IsScopedToProvider()
    {
        var provider1 = CreateProvider<ProviderOneService>();
        var provider2 = CreateProvider<ProviderTwoService>();

        CollectionAssert.AreEqual(new[] { typeof(ProviderOneService) }, provider1.GetAllServiceTypes<IService>());
        CollectionAssert.AreEqual(new[] { typeof(ProviderTwoService) }, provider2.GetAllServiceTypes<IService>());
    }

    [TestMethod]
    public void GetAllServices_IsScopedToProvider()
    {
        var provider1 = CreateProvider<ProviderOneService>();
        var provider2 = CreateProvider<ProviderTwoService>();

        Assert.IsInstanceOfType<ProviderOneService>(provider1.GetAllServices<IService>().Single());
        Assert.IsInstanceOfType<ProviderTwoService>(provider2.GetAllServices<IService>().Single());
    }
}
#endif
