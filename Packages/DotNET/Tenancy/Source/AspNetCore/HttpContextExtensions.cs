// Copyright (c) woksin-org. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.AspNetCore.Http;
using Woksin.Extensions.Tenancy.Context;

namespace Woksin.Extensions.Tenancy.AspNetCore;

/// <summary>
/// Extension methods for getting <see cref="ITenantContext"/> from the <see cref="HttpContext"/>.
/// </summary>
public static class HttpContextExtensions
{
    public static ITenantContext<TTenant> GetTenantContext<TTenant>(this HttpContext context)
        where TTenant : class, ITenantInfo, new()
    {
        ITenantContext<TTenant> result = TenantContext<TTenant>.Unresolved();
        if (context.Items.TryGetValue(TenancyMiddleware.TenantContextItemKey, out var tenantContext) && tenantContext is not null)
        {
            result = tenantContext as ITenantContext<TTenant> ?? throw new InvalidCastException($"Could not cast tenant context of type {tenantContext.GetType()} to {typeof(ITenantContext<TTenant>)}");
        }
        return result;
    }
    
    public static ITenantContext GetTenantContext(this HttpContext context)
    {
        ITenantContext result = TenantContext.Unresolved();
        if (context.Items.TryGetValue(TenancyMiddleware.TenantContextItemKey, out var tenantContext) && tenantContext is not null)
        {
            result = tenantContext as ITenantContext ?? throw new InvalidCastException($"Could not cast tenant context of type {tenantContext.GetType()} to {typeof(ITenantContext)}");
        }
        return result;
    }

    public static ITenantContext GetTenantContext(this HttpRequest request) => request.HttpContext.GetTenantContext();
    public static ITenantContext<TTenant> GetTenantContext<TTenant>(this HttpRequest request) 
        where TTenant : class, ITenantInfo, new() => request.HttpContext.GetTenantContext<TTenant>();

}
