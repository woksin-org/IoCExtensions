// Copyright (c) woksin-org. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Woksin.Extensions.IoC.Tenancy;

/// <summary>
/// Defines an system that knows about the <see cref="IServiceProvider"/> for specific each tenant.
/// </summary>
public interface ITenantScopedServiceProviders : IDisposable
{
    /// <summary>
    /// Gets the <see cref="IServiceProvider"/> for a specific <see cref="TenantId"/>.
    /// </summary>
    /// <param name="tenant">The <see cref="TenantId"/>.</param>
    /// <returns>The <see cref="IServiceProvider"/> containing all specific services associated with the given <see cref="TenantId"/>.</returns>
    /// <remarks>If the <see cref="IServiceProvider"/> for the tenant has not been created it will be created.</remarks>
    IServiceProvider ForTenant(TenantId tenant);
}
