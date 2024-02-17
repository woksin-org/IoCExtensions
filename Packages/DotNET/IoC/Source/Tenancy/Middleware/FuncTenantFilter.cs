// Copyright (c) woksin-org. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.AspNetCore.Http;

namespace Woksin.Extensions.IoC.Tenancy.Middleware;

/// <summary>
/// Represents an implementation of <see cref="ITenantIdFilter"/> that filters based on a given function.
/// </summary>
public class FuncTenantFilter : ITenantIdFilter
{
    readonly Func<HttpContext, TenantId, Task<(bool Include, string ExcludeReason)>> _func;

    public FuncTenantFilter(Func<HttpContext, TenantId, (bool Include, string ExcludeReason)> func)
        : this((context, tenantId) => Task.FromResult(func(context, tenantId)))
    {
    }
    public FuncTenantFilter(Func<HttpContext, TenantId, Task<(bool Include, string ExcludeReason)>> func)
    {
        _func = func;
    }

    /// <inheritdoc />
    public Task<(bool Include, string ExcludeReason)> Filter(HttpContext context, TenantId tenantId) => _func(context, tenantId);
}
