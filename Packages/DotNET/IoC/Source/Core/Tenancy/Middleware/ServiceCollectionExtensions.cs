// Copyright (c) woksin-org. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Extensions.DependencyInjection;

namespace Woksin.Extensions.IoC.Tenancy.Middleware;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds a <see cref="ICanGetTenantIdFromHttpContext"/> tenant id strategy to be used when resolving the <see cref="TenantId"/> for each request.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/>.</param>
    /// <typeparam name="TStrategy">The type of the <see cref="ICanGetTenantIdFromHttpContext" /> tenant id strategy to add.</typeparam>
    /// <returns>The builder for continuation.</returns>
    /// <remarks>The order of when a tenant id strategy is added does matter. Strategies are used in the order that they are gathered.</remarks>
    public static IServiceCollection AddCustomTenantIdStrategy<TStrategy>(this IServiceCollection services) where TStrategy : class, ICanGetTenantIdFromHttpContext
        => services.AddScoped<ICanGetTenantIdFromHttpContext, TStrategy>();

    /// <summary>
    /// Adds a <see cref="ICanGetTenantIdFromHttpContext"/> tenant id strategy to be used when resolving the <see cref="TenantId"/> for each request.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/>.</param>
    /// <param name="strategyFactory">The <see cref="ICanGetTenantIdFromHttpContext"/> factory.</param>
    /// <typeparam name="TStrategy">The type of the <see cref="ICanGetTenantIdFromHttpContext" /> tenant id strategy to add.</typeparam>
    /// <returns>The builder for continuation.</returns>
    /// <remarks>The order of when a tenant id strategy is added does matter. Strategies are used in the order that they are gathered.</remarks>
    public static IServiceCollection AddCustomTenantIdStrategy<TStrategy>(this IServiceCollection services, Func<IServiceProvider, TStrategy> strategyFactory) where TStrategy : class, ICanGetTenantIdFromHttpContext
        => services.AddScoped<ICanGetTenantIdFromHttpContext, TStrategy>(strategyFactory);
    
    /// <summary>
    /// Adds a <see cref="ICanGetTenantIdFromHttpContext"/> tenant id strategy to be used when resolving the <see cref="TenantId"/> for each request.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/>.</param>
    /// <param name="instance">The singleton instance of the <see cref="ICanGetTenantIdFromHttpContext"/>.</param>
    /// <returns>The builder for continuation.</returns>
    /// <remarks>The order of when a tenant id strategy is added does matter. Strategies are used in the order that they are gathered.</remarks>
    public static IServiceCollection AddCustomTenantIdStrategy(this IServiceCollection services, ICanGetTenantIdFromHttpContext instance)
        => services.AddSingleton(instance);
}
