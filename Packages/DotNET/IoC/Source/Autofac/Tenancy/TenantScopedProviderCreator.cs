// Copyright (c) woksin-org. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Woksin.Extensions.IoC.Tenancy;

namespace Woksin.Extensions.IoC.Autofac.Tenancy;

/// <summary>
/// Represents a default implementation of <see cref="TenantScopedProviderCreator{T}"/> providing tenant scoped container by utilizing Autofac.
/// </summary>
sealed class TenantScopedProviderCreator : TenantScopedProviderCreator<AutofacServiceProvider>
{
    readonly IEnumerable<ConfigureTenantContainer> _containerConfigurations;

    public TenantScopedProviderCreator(
        IEnumerable<ConfigureTenantServices> serviceConfigurations,
        IEnumerable<ConfigureTenantContainer> containerConfigurations)
        : base(serviceConfigurations)
    {
        _containerConfigurations = containerConfigurations;
    }

    /// <inheritdoc />
    protected override IServiceProvider CreateFromContainer(AutofacServiceProvider container, TenantId tenant, IServiceCollection tenantServices)
        => new AutofacServiceProvider(container.LifetimeScope.BeginLifetimeScope(builder =>
        {
            builder.Populate(tenantServices);
            foreach (var configure in _containerConfigurations)
            {
                configure?.Invoke(tenant, builder);
            }
        }));
}
