// Copyright (c) woksin-org. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Concurrent;
using Microsoft.Extensions.DependencyInjection;

namespace Woksin.Extensions.IoC.Tenancy;

/// <summary>
/// Represents an implementation of <see cref="ITenantScopedServiceProviders" />.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="TenantScopedServiceProviders"/> class.
/// </remarks>
public sealed class TenantScopedServiceProviders(IServiceProvider rootProvider, ICreateTenantScopedProviders tenantScopedProviderCreator) : ITenantScopedServiceProviders
{
    readonly IServiceProvider _rootProvider = rootProvider;
    readonly ConcurrentDictionary<TenantId, IServiceProvider> _serviceProviders = new();
    readonly ICreateTenantScopedProviders _tenantScopedProviderCreator = tenantScopedProviderCreator;

    /// <inheritdoc />
    public IServiceProvider ForTenant(TenantId tenant)
    {
        if (string.IsNullOrEmpty(tenant))
        {
            throw new ArgumentException("Cannot resolve scoped service provider for empty tenant id", nameof(tenant));
        }
        return _serviceProviders.GetOrAdd(tenant, CreateTenantContainer);
    }

    /// <inheritdoc />
    public void Dispose()
    {
        foreach (var (_, provider) in _serviceProviders)
        {
            (provider as IDisposable)?.Dispose();
        }
        _serviceProviders.Clear();
    }

    IServiceProvider CreateTenantContainer(TenantId tenant)
    {
        var services = new ServiceCollection();
        services.AddSingleton(tenant);
        return _tenantScopedProviderCreator.Create(_rootProvider, tenant, services);
    }
}
