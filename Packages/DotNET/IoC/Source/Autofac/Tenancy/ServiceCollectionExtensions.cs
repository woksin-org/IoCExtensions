// Copyright (c) woksin-org. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Autofac;
using Microsoft.Extensions.DependencyInjection;
using Woksin.Extensions.IoC.Tenancy;
using IServiceCollection = Microsoft.Extensions.DependencyInjection.IServiceCollection;

namespace Woksin.Extensions.IoC.Autofac.Tenancy;

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
    public static IServiceCollection AddTenantScopedServices(this IServiceCollection services, Action<ContainerBuilder> configureTenantServices)
        => services
            .AddSingleton<ConfigureTenantContainer>((_, tenantServices) => configureTenantServices?.Invoke(tenantServices));

    /// <summary>
    /// Add tenant scoped services for specific tenants only.
    /// </summary>
    /// <param name="services">The root <see cref="IServiceCollection"/>.</param>
    /// <param name="configureTenantServices">The callbacks to configure tne tenant scoped services for the given tenants.</param>
    /// <returns>The builder for continuation.</returns>
    public static IServiceCollection AddTenantScopedServices(this IServiceCollection services, params (TenantId TenantId, Action<ContainerBuilder> Configure)[] configureTenantServices)
        => services
            .AddSingleton<ConfigureTenantContainer>((tenantId, tenantServices) =>
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
    public static IServiceCollection AddTenantScopedServices(this IServiceCollection services, ConfigureTenantContainer configureTenantServices)
        => services.AddSingleton(configureTenantServices);
}
