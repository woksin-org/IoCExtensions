// Copyright (c) woksin-org. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Extensions.DependencyInjection;

namespace Woksin.Extensions.IoC.Tenancy;

/// <summary>
/// Extension methods for <see cref="IServiceCollection"/>.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Add tenant scoped services.
    /// </summary>
    /// <param name="services">The root <see cref="IServiceCollection"/>.</param>
    /// <param name="configureTenantServices">The callback to configure tne tenant scoped services.</param>
    /// <returns>The builder for continuation.</returns>
    public static IServiceCollection AddTenantScopedServices(this IServiceCollection services, Action<IServiceCollection> configureTenantServices)
        => services
            .AddSingleton<ConfigureTenantServices>((_, tenantServices) => configureTenantServices?.Invoke(tenantServices));

    /// <summary>
    /// Add tenant scoped services for specific tenants only.
    /// </summary>
    /// <param name="services">The root <see cref="IServiceCollection"/>.</param>
    /// <param name="configureTenantServices">The callbacks to configure tne tenant scoped services for the given tenants.</param>
    /// <returns>The builder for continuation.</returns>
    public static IServiceCollection AddTenantScopedServices(this IServiceCollection services, params (TenantId TenantId, Action<IServiceCollection> Configure)[] configureTenantServices)
        => services
            .AddSingleton<ConfigureTenantServices>((tenantId, tenantServices) =>
            {
                foreach (var configure in configureTenantServices.Where(_ => _.TenantId == tenantId).Select(_ => _.Configure))
                {
                    configure?.Invoke(tenantServices);
                }
            });

    /// <summary>
    /// Add tenant scoped services.
    /// </summary>
    /// <param name="services">The root <see cref="IServiceCollection"/>.</param>
    /// <param name="configureTenantServices">The callback to configure tne tenant scoped services.</param>
    /// <returns>The builder for continuation.</returns>
    public static IServiceCollection AddTenantScopedServices(this IServiceCollection services, ConfigureTenantServices configureTenantServices)
        => services.AddSingleton(configureTenantServices);
}
