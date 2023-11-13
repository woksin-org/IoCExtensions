// Copyright (c) woksin-org. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Woksin.Extensions.IoC.Tenancy.Middleware;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds a <see cref="ICanGetTenantIdFromHttpContext"/> tenant id strategy to be used when resolving the <see cref="TenantId"/> for each request.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/>.</param>
    /// <param name="lifetime">The optional <see cref="ServiceLifetime"/> of the registered strategy.</param>
    /// <typeparam name="TStrategy">The type of the <see cref="ICanGetTenantIdFromHttpContext" /> tenant id strategy to add.</typeparam>
    /// <returns>The builder for continuation.</returns>
    /// <remarks>The order of when a tenant id strategy is added does matter. Strategies are used in the order that they are registered.</remarks>
    public static IServiceCollection AddCustomTenantIdStrategy<TStrategy>(this IServiceCollection services, ServiceLifetime lifetime = ServiceLifetime.Scoped) where TStrategy : class, ICanGetTenantIdFromHttpContext
    {
        services.Add(new ServiceDescriptor(typeof(ICanGetTenantIdFromHttpContext), typeof(TStrategy), lifetime));
        return services;
    }

    /// <summary>
    /// Adds a <see cref="ICanGetTenantIdFromHttpContext"/> tenant id strategy to be used when resolving the <see cref="TenantId"/> for each request.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/>.</param>
    /// <param name="instance">The singleton instance of the <see cref="ICanGetTenantIdFromHttpContext"/>.</param>
    /// <returns>The builder for continuation.</returns>
    /// <remarks>The order of when a tenant id strategy is added does matter. Strategies are used in the order that they are registered.</remarks>
    public static IServiceCollection AddCustomTenantIdStrategy(this IServiceCollection services, ICanGetTenantIdFromHttpContext instance)
        => services.AddSingleton(instance);

    /// <summary>
    /// Adds a <see cref="ITenantIdFilter"/> tenant id filter to be used when getting the tenant scoped <see cref="IServiceProvider"/>.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/>.</param>
    /// <param name="lifetime">The optional <see cref="ServiceLifetime"/> of the registered filter.</param>
    /// <typeparam name="TFilter">The type of the <see cref="ITenantIdFilter" /> tenant id filter to add.</typeparam>
    /// <returns>The builder for continuation.</returns>
    public static IServiceCollection AddTenantIdFilter<TFilter>(this IServiceCollection services, ServiceLifetime lifetime = ServiceLifetime.Scoped) where TFilter : class, ITenantIdFilter
    {
        services.Add(new ServiceDescriptor(typeof(ITenantIdFilter), typeof(TFilter), lifetime));
        return services;
    }

    /// <summary>
    /// Adds a <see cref="ITenantIdFilter"/> tenant id filter to be used when getting the tenant scoped <see cref="IServiceProvider"/>.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/>.</param>
    /// <param name="filter">The <see cref="ITenantIdFilter"/> to add.</param>
    /// <typeparam name="TFilter">The type of the <see cref="ITenantIdFilter" /> tenant id filter to add.</typeparam>
    /// <returns>The builder for continuation.</returns>
    public static IServiceCollection AddTenantIdFilter<TFilter>(this IServiceCollection services, TFilter filter) where TFilter : class, ITenantIdFilter
        => services.AddSingleton<ITenantIdFilter>(filter);

    /// <summary>
    /// Adds a <see cref="ITenantIdFilter"/> tenant id filter to be used when getting the tenant scoped <see cref="IServiceProvider"/>.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/>.</param>
    /// <param name="filter">The func to base the <see cref="FuncTenantFilter"/> on.</param>
    /// <returns>The builder for continuation.</returns>
    public static IServiceCollection AddTenantIdFilter(this IServiceCollection services, Func<HttpContext, TenantId, (bool, string)> filter)
        => services.AddSingleton<ITenantIdFilter>(new FuncTenantFilter(filter));

    /// <summary>
    /// Adds a <see cref="ITenantIdFilter"/> tenant id filter to be used when getting the tenant scoped <see cref="IServiceProvider"/>.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/>.</param>
    /// <param name="filter">The func to base the <see cref="FuncTenantFilter"/> on.</param>
    /// <returns>The builder for continuation.</returns>
    public static IServiceCollection AddTenantIdFilter(this IServiceCollection services, Func<HttpContext, TenantId, Task<(bool, string)>> filter)
        => services.AddSingleton<ITenantIdFilter>(new FuncTenantFilter(filter));

    /// <summary>
    /// Adds a <see cref="ITenantIdFilter"/> tenant id filter to be used when getting the tenant scoped <see cref="IServiceProvider"/> that only allows the specified tenant ids.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/>.</param>
    /// <param name="tenantIds">The tenant ids to allow.</param>
    /// <returns>The builder for continuation.</returns>
    public static IServiceCollection OnlyAllowTenantIds(this IServiceCollection services, params TenantId[] tenantIds)
        => services.AddSingleton<ITenantIdFilter>(new FuncTenantFilter(
            (_, tenantId) => (tenantIds.Contains(tenantId), $"Tenant Id '{tenantId}' is not allowed.")));
    
    /// <summary>
    /// Adds a <see cref="ITenantIdFilter"/> tenant id filter to be used when getting the tenant scoped <see cref="IServiceProvider"/> that excludes the specified tenant ids.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/>.</param>
    /// <param name="tenantIds">The tenant ids to exclude.</param>
    /// <returns>The builder for continuation.</returns>
    public static IServiceCollection ExcludeTenantIds(this IServiceCollection services, params TenantId[] tenantIds)
        => services.AddSingleton<ITenantIdFilter>(new FuncTenantFilter(
            (_, tenantId) => (!tenantIds.Contains(tenantId), $"Tenant Id '{tenantId}' is not allowed.")));
}
