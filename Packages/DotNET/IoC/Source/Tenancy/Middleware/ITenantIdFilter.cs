// Copyright (c) woksin-org. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.AspNetCore.Http;

namespace Woksin.Extensions.IoC.Tenancy.Middleware;

/// <summary>
/// Defines a <see cref="TenantId"/> that can filter out if the given tenant id is bad.
/// </summary>
/// <remarks>This filter is only used in the <see cref="TenantScopedServiceProviderMiddleware"/>.</remarks>
public interface ITenantIdFilter
{
    /// <summary>
    /// Filters the Tenant Id, determine whether it is to be included.
    /// </summary>
    /// <param name="context">The <see cref="HttpContext"/> that included the <see cref="TenantId"/>.</param>
    /// <param name="tenantId">The <see cref="TenantId"/> to filter.</param>
    /// <returns>Tuple with whether it succeeded and a reason for why it was not included.</returns>
    public Task<(bool Include, string ExcludeReason)> Filter(HttpContext context, TenantId tenantId);
}
