// Copyright (c) woksin-org. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Extensions.DependencyInjection;

namespace Woksin.Extensions.IoC.Tenancy;

/// <summary>
/// Represents a base implementation of <see cref="ICreateTenantScopedProviders"/> that can be used to specify how to build Tenant-specific IoC containers.
/// </summary>
/// <typeparam name="TContainer">The type of the container.</typeparam>
public abstract class TenantScopedProviderCreator<TContainer> : ICreateTenantScopedProviders
{
    readonly IEnumerable<ConfigureTenantServices> _serviceConfigurations;

    protected TenantScopedProviderCreator(IEnumerable<ConfigureTenantServices> serviceConfigurations)
    {
        _serviceConfigurations = serviceConfigurations;
    }

    /// <inheritdoc />
    public IServiceProvider Create(IServiceProvider rootProvider, TenantId tenant, IServiceCollection tenantServices)
    {
        foreach (var configureServices in _serviceConfigurations)
        {
            configureServices?.Invoke(tenant, tenantServices);
        }
        var container = ContainerFrom(rootProvider);
        return CreateFromContainer(container, tenant, tenantServices);
    }

    /// <summary>
    /// Creates a Tenant-specific IoC container from the root container, for the specified tenant with the Tenant-specific services.
    /// </summary>
    /// <param name="container">The root <typeparamref name="TContainer"/> to use.</param>
    /// <param name="tenant">The <see cref="TenantId"/> to create the IoC container for.</param>
    /// <param name="tenantServices">The Tenant-specific <see cref="IServiceCollection"/> to populate the container with.</param>
    /// <returns>An <see cref="IServiceProvider"/> to use for the specified tenant.</returns>
    protected abstract IServiceProvider CreateFromContainer(TContainer container, TenantId tenant, IServiceCollection tenantServices);

    /// <summary>
    /// Resolves the <typeparamref name="TContainer"/> from the root <see cref="IServiceProvider"/>.
    /// </summary>
    /// <param name="provider">The <see cref="IServiceProvider"/> to resolve the root container from.</param>
    /// <returns>The root <typeparamref name="TContainer"/>.</returns>
    /// <exception cref="CannotResolveContainerFromServiceProvider"></exception>
    static TContainer ContainerFrom(IServiceProvider provider)
    {
        if (provider is TContainer container)
        {
            return container;
        }

        throw new CannotResolveContainerFromServiceProvider(typeof(TContainer), provider);
    }
}
