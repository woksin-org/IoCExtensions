// Copyright (c) woksin-org. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Extensions.DependencyInjection;

namespace Woksin.Extensions.IoC.Tenancy;

/// <summary>
/// Defines a system that can create a Tenant-specific <see cref="IServiceProvider"/> from the root <see cref="IServiceProvider"/>.
/// </summary>
public interface ICreateTenantScopedProviders
{
    /// <summary>
    /// Creates the <see cref="IServiceProvider"/> for a specific tenant using the root <see cref="IServiceProvider"/> and a set of tenant specific services.
    /// </summary>
    /// <param name="rootProvider">The root <see cref="IServiceProvider"/> that all the tenant specific <see cref="IServiceProvider"/> instances are based off.</param>
    /// <param name="tenant">The <see cref="TenantId"/> to create a container for.</param>
    /// <param name="tenantServices">The <see cref="IServiceCollection"/> of services to configure for the tenant.</param>
    /// <returns>The <see cref="IServiceProvider"/> to use for the specified tenant.</returns>
    IServiceProvider Create(IServiceProvider rootProvider, TenantId tenant, IServiceCollection tenantServices);
}
