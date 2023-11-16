// Copyright (c) woksin-org. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Woksin.Extensions.IoC.Tenancy.Middleware;

/// <summary>
/// Extension methods for <see cref="IApplicationBuilder"/>.
/// </summary>
public static class ApplicationBuilderExtensions
{
    /// <summary>
    /// <para>
    /// Use <see cref="ICanGetTenantIdFromHttpContext"/> to resolve the <see cref="TenantId"/> from <see cref="HttpContext"/> using the <see cref="TenantScopedServiceProviderMiddleware"/>.
    /// The <see cref="TenantScopedServiceProviderMiddleware"/> uses the resolved <see cref="TenantId"/> to set the <see cref="HttpContext.RequestServices"/> to be the tenant-scoped <see cref="IServiceProvider"/>
    /// provided from the <see cref="ITenantScopedServiceProviders"/>.
    /// </para>
    /// <para>
    /// Add custom <see cref="ICanGetTenantIdFromHttpContext"/> implementations by using the AddTenantIdStrategy extension methods on the <see cref="IServiceCollection"/>.
    /// </para>
    /// </summary>
    /// <param name="builder">The <see cref="IApplicationBuilder"/>.</param>
    /// <returns>The <see cref="IApplicationBuilder"/> for continuation.</returns>
    /// <remarks>If no custom <see cref="ICanGetTenantIdFromHttpContext"/> are added the <see cref="TenantScopedServiceProviderMiddleware"/> will by default use <see cref="TenantIdFromHeaderStrategy.Default"/></remarks>
    public static IApplicationBuilder UseTenantIdStrategies(this IApplicationBuilder builder)
        => builder.UseMiddleware<TenantScopedServiceProviderMiddleware>();
}
