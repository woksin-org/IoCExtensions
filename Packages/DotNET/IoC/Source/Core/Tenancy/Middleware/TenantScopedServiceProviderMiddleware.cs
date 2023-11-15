// Copyright (c) woksin-org. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Woksin.Extensions.IoC.Tenancy.Middleware;

/// <summary>
/// Represents a middleware that intercepts <see cref="HttpRequest"/> and sets the <see cref="IServiceProvider"/> to a scoped
/// <see cref="IServiceProvider"/> that knows about the tenant-scoped services based on configured stategies
/// </summary>
public partial class TenantScopedServiceProviderMiddleware
{
    readonly RequestDelegate _next;

    static readonly IEnumerable<ICanGetTenantIdFromHttpContext> _defaultStrategies = new[]
    {
        TenantIdFromHeaderStrategy.Default
    };

    /// <summary>
    /// Initializes a new instance of the <see cref="TenantScopedServiceProviderMiddleware"/> class.
    /// </summary>
    /// <param name="next">The <see cref="RequestDelegate"/>.</param>
    public TenantScopedServiceProviderMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    /// <summary>
    /// Invokes the middleware.
    /// </summary>
    /// <param name="context">The <see cref="HttpContext"/>.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public async Task InvokeAsync(HttpContext context)
    {
        var logger = context.RequestServices.GetService<ILogger<TenantScopedServiceProviderMiddleware>>() ?? NullLogger<TenantScopedServiceProviderMiddleware>.Instance;
        var tenantId = await ResolveTenantId(context, logger).ConfigureAwait(false);
        if (tenantId is null)
        {
            LogNoResolvedTenantId(logger);
            await _next(context).ConfigureAwait(false);
            return;
        }
        LogResolvedTenantId(logger, tenantId);
        var scope = context.RequestServices.GetTenantScopedProvider(tenantId).CreateAsyncScope();
        await using var _ = scope.ConfigureAwait(false);
        context.RequestServices = scope.ServiceProvider;
        await _next(context).ConfigureAwait(false);
    }

    static async Task<TenantId?> ResolveTenantId(HttpContext context, ILogger logger)
    {
        var tenantIdStrategies = context.RequestServices.GetRequiredService<IEnumerable<ICanGetTenantIdFromHttpContext>>().ToArray();
        var tenantIdFilters = context.RequestServices.GetRequiredService<IEnumerable<ITenantIdFilter>>().ToArray();
        if (tenantIdStrategies.Length == 0)
        {
            tenantIdStrategies = _defaultStrategies.ToArray();
        }

        foreach (var strategy in tenantIdStrategies)
        {
            var tenantId = await TryGetTenantId(strategy, context, logger).ConfigureAwait(false);
            if (tenantId is null)
            {
                continue;
            }
            if (await ShouldUseTenantId(tenantIdFilters, context, tenantId, logger).ConfigureAwait(false))
            {
                return tenantId;
            }
        }
        return null;
    }

    static async Task<TenantId?> TryGetTenantId(ICanGetTenantIdFromHttpContext strategy, HttpContext context, ILogger logger)
    {
        try
        {
            var result = await strategy.GetAsync(context).ConfigureAwait(false);
            return result.TenantId;
        }
        catch (Exception ex)
        {
            LogErrorGettingTenantId(logger, ex, strategy.GetType());
            return null;
        }
    }

    static async Task<bool> ShouldUseTenantId(IEnumerable<ITenantIdFilter> tenantIdFilters, HttpContext context, TenantId tenantId, ILogger logger)
    {
        try
        {
            var filterResults = await Task.WhenAll(tenantIdFilters.Select(filter => filter.Filter(context, tenantId))).ConfigureAwait(false);
            if (filterResults.All(r => r.Include))
            {
                return true;
            }
            foreach (var (_, excludeReason) in filterResults.Where(result => !result.Include))
            {
                LogTenantIdExcluded(logger, tenantId, excludeReason);
            }
        }
        catch (Exception e)
        {
            LogErrorFilteringTenantId(logger, e, tenantId);
        }

        return false;
    }

    [LoggerMessage(0, LogLevel.Debug, "Resolved tenant id {TenantId} from Http Context. Using it as the tenant scoped service provider.")]
    static partial void LogResolvedTenantId(ILogger logger, TenantId tenantId);

    [LoggerMessage(1, LogLevel.Warning, "No tenant id resolved. Tenant scoped provider is not being used for this request")]
    static partial void LogNoResolvedTenantId(ILogger logger);

    [LoggerMessage(2, LogLevel.Warning, "An error occurred while resolving tenant id. {StrategyType} failed with an error")]
    static partial void LogErrorGettingTenantId(ILogger logger, Exception error, Type strategyType);

    [LoggerMessage(3, LogLevel.Warning, "An error occurred while filtering tenant id {TenantId}")]
    static partial void LogErrorFilteringTenantId(ILogger logger, Exception error, TenantId tenantId);

    [LoggerMessage(4, LogLevel.Warning, "Tenant id {TenantId} is excluded by tenant id filter. {ExclusionReason}")]
    static partial void LogTenantIdExcluded(ILogger logger, TenantId tenantId, string exclusionReason);
}
