namespace VoidNone.DependencyInjectionTest;

/// <summary>
/// Tests for <see cref="SingletonAttribute"/>, <see cref="ScopedAttribute"/>, and <see cref="TransientAttribute"/>.
/// </summary>
[TestClass]
public class LifetimeAttributeTest
{
    [Singleton]
    private class SingletonService { }

    [Scoped]
    private class ScopedService { }

    [Transient]
    private class TransientService { }

    [Lifetime(ServiceLifetime.Singleton)]
    private class LifetimeSingletonService { }

    [Lifetime(ServiceLifetime.Scoped)]
    private class LifetimeScopedService { }

    [Lifetime(ServiceLifetime.Transient)]
    private class LifetimeTransientService { }

    private static IServiceProvider CreateServiceProvider(params Type[] types)
    {
        return new ServiceCollection()
            .AddFromAssemblies(typeof(LifetimeAttributeTest).Assembly)
            .BuildServiceProvider();
    }

    [TestMethod]
    public void Singleton_CreatesSameInstance()
    {
        var provider = CreateServiceProvider();
        var service1 = provider.GetService<SingletonService>();
        var service2 = provider.GetService<SingletonService>();

        Assert.IsNotNull(service1);
        Assert.IsNotNull(service2);
        Assert.AreSame(service1, service2);
    }

    [TestMethod]
    public void Scoped_CreatesSameInstanceWithinScope()
    {
        var provider = CreateServiceProvider();

        using var scope1 = provider.CreateScope();
        using var scope2 = provider.CreateScope();

        var service1 = scope1.ServiceProvider.GetService<ScopedService>();
        var service2 = scope1.ServiceProvider.GetService<ScopedService>();
        var service3 = scope2.ServiceProvider.GetService<ScopedService>();

        Assert.IsNotNull(service1);
        Assert.IsNotNull(service2);
        Assert.IsNotNull(service3);
        Assert.AreSame(service1, service2);
        Assert.AreNotSame(service1, service3);
    }

    [TestMethod]
    public void Transient_CreatesNewInstanceEachTime()
    {
        var provider = CreateServiceProvider();

        var service1 = provider.GetService<TransientService>();
        var service2 = provider.GetService<TransientService>();

        Assert.IsNotNull(service1);
        Assert.IsNotNull(service2);
        Assert.AreNotSame(service1, service2);
    }

    [TestMethod]
    public void LifetimeAttribute_BaseClass_Works()
    {
        var provider = CreateServiceProvider();

        var singleton = provider.GetService<LifetimeSingletonService>();
        var scoped = provider.GetService<LifetimeScopedService>();
        var transient = provider.GetService<LifetimeTransientService>();

        Assert.IsNotNull(singleton);
        Assert.IsNotNull(scoped);
        Assert.IsNotNull(transient);
    }
}
