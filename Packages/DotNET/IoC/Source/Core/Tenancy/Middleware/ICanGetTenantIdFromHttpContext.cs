// Copyright (c) woksin-org. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.AspNetCore.Http;

namespace Woksin.Extensions.IoC.Tenancy.Middleware;

/// <summary>
/// Defines a strategy for retrieving <see cref="TenantId"/> from <see cref="HttpContext"/>.
/// </summary>
public interface ICanGetTenantIdFromHttpContext
{
    /// <summary>
    /// Try get <see cref="TenantId"/> from <see cref="HttpContext"/>.
    /// </summary>
    /// <param name="context">The <see cref="HttpContext"/>.</param>
    /// <returns>Returns a tuple representing whether the <see cref="TenantId"/> was successfully retrieved.</returns>
    public Task<(bool Success, TenantId? TenantId)> GetAsync(HttpContext context);
}
