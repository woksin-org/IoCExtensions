// Copyright (c) woksin-org. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Woksin.Extensions.Tenancy.Context;

namespace Woksin.Extensions.Tenancy.AspNetCore;

/// <summary>
/// Represents the middleware that resolves the tenant context and sets it on the <see cref="ITenantContextAccessor.CurrentTenant"/>.
/// </summary>
public partial class TenancyMiddleware
{
    readonly RequestDelegate _next;

    public TenancyMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        var logger = context.RequestServices.GetService<ILogger<TenancyMiddleware>>() ?? NullLogger<TenancyMiddleware>.Instance;
        LogGettingTenantContext(logger);
        var accessor = context.RequestServices.GetRequiredService<ITenantContextAccessor>();
        if (!accessor.CurrentTenant.Resolved(out var tenantInfo, out _))
        {
            LogResolvingTenantContext(logger);
            var resolver = context.RequestServices.GetRequiredService<IResolveTenant>();
            accessor.CurrentTenant = await resolver.Resolve(context);
        }
        else
        {
            LogTenantContextExists(logger, tenantInfo.Id, tenantInfo.Name);
        }

        await _next(context);
    }

    [LoggerMessage(0, LogLevel.Debug, "Getting current tenant context")]
    static partial void LogGettingTenantContext(ILogger logger);

    [LoggerMessage(1, LogLevel.Debug, "Not in the context of a tenant. Resolving tenant identifier")]
    static partial void LogResolvingTenantContext(ILogger logger);

    [LoggerMessage(2, LogLevel.Debug, "Tenant context was already resolved to id {TenantIdentifier} with name {TenantName}")]
    static partial void LogTenantContextExists(ILogger logger, string tenantIdentifier, string? tenantName);
}
