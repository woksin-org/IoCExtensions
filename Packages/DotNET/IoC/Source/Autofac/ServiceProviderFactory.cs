// Copyright (c) woksin-org. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Woksin.Extensions.IoC.Provider;
using Woksin.Extensions.IoC.Registry;

namespace Woksin.Extensions.IoC.Autofac;

/// <summary>
/// Represents an implementation of <see cref="IServiceProviderFactory{TContainerBuilder}"/> for the Autofac <see cref="ContainerBuilder"/> that sets up the Service Provider using Autofac.
/// </summary>
class ServiceProviderFactory(Action<ContainerBuilder>? configureContainer) : IoCExtensionsServiceProviderFactory<ContainerBuilder>
{
    readonly AutofacServiceProviderFactory _factory = new();

    /// <inheritdoc />
    protected override ContainerBuilder CreateContainerBuilder(IServiceCollection services) =>
	    _factory.CreateBuilder(services);

    /// <inheritdoc />
    protected override IServiceProvider CreateServiceProvider(
        ContainerBuilder containerBuilder,
	    DiscoveredServices<ContainerBuilder> discoveredServices)
    {
        containerBuilder.RegisterClassesByLifecycle(
            [.. discoveredServices.ClassesToRegister.SingletonClasses],
            [.. discoveredServices.ClassesToRegister.ScopedClasses],
            [.. discoveredServices.ClassesToRegister.TransientClasses]);

        containerBuilder.RegisterClassesByLifecycleAsSelf(
            [.. discoveredServices.ClassesToRegisterAsSelf.SingletonClasses],
            [.. discoveredServices.ClassesToRegisterAsSelf.ScopedClasses],
            [.. discoveredServices.ClassesToRegisterAsSelf.TransientClasses]);

	    containerBuilder.Populate(discoveredServices.AdditionalServices);
        containerBuilder.RegisterAssemblyModules([.. discoveredServices.Assemblies]);
        configureContainer?.Invoke(containerBuilder);
        return _factory.CreateServiceProvider(containerBuilder);
    }
}
