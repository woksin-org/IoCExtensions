using Lamar;
using IoCExtensions.Provider;
using IoCExtensions.Registry;
using Lamar.Microsoft.DependencyInjection;
using Lamar.Scanning.Conventions;
using Microsoft.Extensions.DependencyInjection;

namespace IoCExtensions.Lamar;

/// <summary>
/// Represents an implementation of <see cref="IServiceProviderFactory{TContainer}"/> for the Lamar <see cref="ServiceRegistry"/> that sets up the Service Provider using Lamar.
/// </summary>
class ServiceProviderFactory : IoCExtensionsServiceProviderFactory<ServiceRegistry>
{
	readonly Action<ServiceRegistry>? _configureContainer;
	readonly LamarServiceProviderFactory _factory = new();

    public ServiceProviderFactory(Action<ServiceRegistry>? configureContainer)
    {
	    _configureContainer = configureContainer;
    }

    /// <inheritdoc />
    protected override ServiceRegistry CreateContainerBuilder(IServiceCollection services) =>
	    _factory.CreateBuilder(services);

    /// <inheritdoc />
    protected override IServiceProvider CreateServiceProvider(ServiceRegistry containerBuilder,
	    DiscoveredServices<ServiceRegistry> discoveredServices)
    {
        containerBuilder.AddRange(discoveredServices.AdditionalServices);
        containerBuilder.RegisterClassesByLifecycle(discoveredServices.ClassesToRegister);
        containerBuilder.RegisterClassesByLifecycleAsSelf(discoveredServices.ClassesToRegisterAsSelf);
        containerBuilder.Scan(_ =>
        {
            foreach (var assembly in discoveredServices.Assemblies)
            {
                _.Assembly(assembly);
            }
            _.LookForRegistries();
        });
        _configureContainer?.Invoke(containerBuilder);
        return _factory.CreateServiceProvider(containerBuilder);
    }
}
