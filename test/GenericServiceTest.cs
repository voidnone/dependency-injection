namespace VoidNone.DependencyInjection.Tests;

/// <summary>
/// Tests for generic service registration.
/// </summary>
[TestClass]
public class GenericServiceTest
{
    [Transient(typeof(IService<>))]
    internal class Service<T> : IService<T> { }

    internal interface IService<T> { }

    internal interface IMyService : IService<DateTime> { }

    [Transient(typeof(IMyService))]
    internal class MyService : Service<DateTime>, IMyService { }

    private static IServiceProvider CreateProvider()
    {
        return new ServiceCollection()
            .AddFromAttributes()
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
