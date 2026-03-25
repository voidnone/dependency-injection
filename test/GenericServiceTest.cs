namespace VoidNone.DependencyInjectionTest;

/// <summary>
/// Tests for generic service registration.
/// </summary>
[TestClass]
public class GenericServiceTest
{
    [Transient(typeof(IService<>))]
    private class Service<T> : IService<T> { }

    private interface IService<T> { }

    private interface IMyService : IService<DateTime> { }

    [Transient(typeof(IMyService))]
    private class MyService : Service<DateTime>, IMyService { }

    private static IServiceProvider CreateProvider()
    {
        return new ServiceCollection()
            .AddFromAssemblies(typeof(GenericServiceTest).Assembly)
            .BuildServiceProvider();
    }

    [TestMethod]
    public void OpenGenericService_CanBeResolved()
    {
        var provider = CreateProvider();

        var service = provider.GetService<IService<DateTime>>();

        Assert.IsNotNull(service);
        Assert.IsInstanceOfType(service, typeof(Service<DateTime>));
    }

    [TestMethod]
    public void ClosedGenericService_CanBeResolved()
    {
        var provider = CreateProvider();

        var service = provider.GetService<IMyService>();

        Assert.IsNotNull(service);
        Assert.IsInstanceOfType(service, typeof(MyService));
    }
}
