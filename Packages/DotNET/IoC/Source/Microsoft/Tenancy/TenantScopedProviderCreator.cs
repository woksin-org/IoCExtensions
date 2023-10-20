// Copyright (c) woksin-org. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Woksin.Extensions.IoC.Tenancy;

namespace Woksin.Extensions.IoC.Microsoft.Tenancy;

/// <summary>
/// Represents a default implementation of <see cref="TenantScopedProviderCreator{T}"/> providing tenant scoped container support using the Microsoft DI by utilizing Autofac for the scoped providers.
/// </summary>
sealed class TenantScopedProviderCreator : TenantScopedProviderCreator<IServiceProvider>
{
    public TenantScopedProviderCreator(IEnumerable<ConfigureTenantServices> serviceConfigurations)
        : base(serviceConfigurations)
    {
    }

    /// <inheritdoc />
    protected override IServiceProvider CreateFromContainer(IServiceProvider container, TenantId tenant, IServiceCollection tenantServices)
    {
        var containerBuilder = new ContainerBuilder();
        var registrationSource = new UnknownServiceOnTenantContainerRegistrationSource(container, true);
        containerBuilder.Populate(tenantServices);
        containerBuilder.RegisterSource(registrationSource);
        containerBuilder.Register(context => new ServiceScopeFactory(
            container.GetRequiredService<IServiceScopeFactory>(),
            context.Resolve<ILifetimeScope>())).As<IServiceScopeFactory>().SingleInstance();
        var tenantScopedContainer = containerBuilder.Build();
        registrationSource.Registrations = tenantScopedContainer.ComponentRegistry.Registrations;
        return new AutofacServiceProvider(tenantScopedContainer);
    }
}
