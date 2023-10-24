// Copyright (c) woksin-org. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Woksin.Extensions.IoC.Tenancy.Middleware;

/// <summary>
/// Represents a middleware that intercepts <see cref="HttpRequest"/> and sets the <see cref="IServiceProvider"/> to a scoped
/// <see cref="IServiceProvider"/> that knows about the tenant-scoped services based on configured stategies
/// </summary>
public partial class TenantScopedServiceProviderMiddleware
{
    readonly RequestDelegate _next;
    readonly ILogger<TenantScopedServiceProviderMiddleware> _logger;
    static readonly IEnumerable<ICanGetTenantIdFromHttpContext> _defaultStrategies = new[]
    {
        TenantIdFromHeaderStrategy.Default
    };

    /// <summary>
    /// Initializes a new instance of the <see cref="TenantScopedServiceProviderMiddleware"/> class.
    /// </summary>
    /// <param name="next">The <see cref="RequestDelegate"/>.</param>
    /// <param name="logger">The <see cref="ILogger"/>.</param>
    public TenantScopedServiceProviderMiddleware(RequestDelegate next, ILogger<TenantScopedServiceProviderMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    /// <summary>
    /// Invokes the middleware.
    /// </summary>
    /// <param name="context">The <see cref="HttpContext"/>.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public async Task InvokeAsync(HttpContext context)
    {
        var serviceProvider = context.RequestServices;
        var tenantIdRetrieverStrategies = serviceProvider.GetService<IEnumerable<ICanGetTenantIdFromHttpContext>>() ?? _defaultStrategies;
        TenantId? tenantId = null;
        foreach (var strategy in tenantIdRetrieverStrategies)
        {
            var result = await strategy.GetAsync(context).ConfigureAwait(false);
            if (!result.Success)
            {
                continue;
            }
            tenantId = result.TenantId;
            break;
        }
        if (tenantId is null)
        {
            NoResolvedTenantId();
            await _next(context).ConfigureAwait(false);
            return;
        }

        var scope = serviceProvider.GetTenantScopedProvider(tenantId).CreateAsyncScope();
        await using var _ = scope.ConfigureAwait(false);
        context.RequestServices = scope.ServiceProvider;
        await _next(context).ConfigureAwait(false);
    }

    [LoggerMessage(0, LogLevel.Debug, "Resolved tenant id {TenantId} from Http Context")]
    partial void ResolvedTenantId(TenantId tenantId);

    [LoggerMessage(1, LogLevel.Warning, "No tenant id resolved. Tenant scoped provider is not being used for this request")]
    partial void NoResolvedTenantId();
}
