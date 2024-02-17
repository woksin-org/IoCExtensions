// Copyright (c) woksin-org. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.AspNetCore.Http;

namespace Woksin.Extensions.IoC.Tenancy.Middleware;

/// <summary>
/// Represents an implementation of <see cref="ICanGetTenantIdFromHttpContext"/> that gets the <see cref="TenantId"/> based on a given function.
/// </summary>
public class FuncTenantIdStrategy : ICanGetTenantIdFromHttpContext
{
    readonly Func<HttpContext, Task<(bool Success, TenantId? TenantId)>> _func;

    public FuncTenantIdStrategy(Func<HttpContext, (bool Success, TenantId? TenantId)> func)
        : this(context => Task.FromResult(func(context)))
    {
    }
    public FuncTenantIdStrategy(Func<HttpContext, Task<(bool Success, TenantId? TenantId)>> func)
    {
        _func = func;
    }

    /// <inheritdoc />
    public Task<(bool Success, TenantId? TenantId)> GetAsync(HttpContext context) => _func(context);
}
