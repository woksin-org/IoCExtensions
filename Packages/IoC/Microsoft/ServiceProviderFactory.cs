using IoCExtensions.Provider;
using IoCExtensions.Registry;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace IoCExtensions.Microsoft;

/// <summary>
/// Represents an implementation of <see cref="IServiceProviderFactory{TContainerBuilder}"/> for <see cref="IServiceCollection"/>
/// that sets up the Service Provider using Microsoft Dependency Injection.
/// </summary>
class ServiceProviderFactory : IoCExtensionsServiceProviderFactory<IServiceCollection>
{
	readonly Action<IServiceCollection>? _configureServices;

    public ServiceProviderFactory(Action<IServiceCollection>? configureServices)
    {
	    _configureServices = configureServices;
    }

    /// <inheritdoc />
    protected override IServiceCollection CreateContainerBuilder(IServiceCollection services) => services;

    /// <inheritdoc />
    protected override IServiceProvider CreateServiceProvider(IServiceCollection containerBuilder,
	    DiscoveredServices<IServiceCollection> discoveredServices)
    {
        containerBuilder.Add(discoveredServices.AdditionalServices);
        containerBuilder.RegisterClassesByLifecycle(discoveredServices.ClassesToRegister);
        containerBuilder.RegisterClassesByLifecycleAsSelf(discoveredServices.ClassesToRegisterAsSelf);
        _configureServices?.Invoke(containerBuilder);
        var provider = containerBuilder.BuildServiceProvider();
        return provider;
    }
}
