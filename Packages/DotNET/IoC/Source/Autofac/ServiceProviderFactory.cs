// Copyright (c) woksin-org. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Autofac;
using Autofac.Extensions.DependencyInjection;
using IoCExtensions.Provider;
using IoCExtensions.Registry;
using Microsoft.Extensions.DependencyInjection;

namespace IoCExtensions.Autofac;

/// <summary>
/// Represents an implementation of <see cref="IServiceProviderFactory{TContainerBuilder}"/> for the Autofac <see cref="ContainerBuilder"/> that sets up the Service Provider using Autofac.
/// </summary>
class ServiceProviderFactory : IoCExtensionsServiceProviderFactory<ContainerBuilder>
{
	readonly Action<ContainerBuilder>? _configureContainer;
	readonly AutofacServiceProviderFactory _factory = new();

    public ServiceProviderFactory(Action<ContainerBuilder>? configureContainer)
    {
	    _configureContainer = configureContainer;
    }

    /// <inheritdoc />
    protected override ContainerBuilder CreateContainerBuilder(IServiceCollection services) =>
	    _factory.CreateBuilder(services);

    /// <inheritdoc />
    protected override IServiceProvider CreateServiceProvider(ContainerBuilder containerBuilder,
	    DiscoveredServices<ContainerBuilder> discoveredServices)
    {
	    containerBuilder.Populate(discoveredServices.AdditionalServices);
        containerBuilder.RegisterClassesByLifecycle(discoveredServices.ClassesToRegister);
        containerBuilder.RegisterClassesByLifecycleAsSelf(discoveredServices.ClassesToRegisterAsSelf);
        containerBuilder.RegisterAssemblyModules(discoveredServices.Assemblies.ToArray());
        _configureContainer?.Invoke(containerBuilder);

        return _factory.CreateServiceProvider(containerBuilder);
    }
}
