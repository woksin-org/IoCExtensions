// Copyright (c) woksin-org. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Woksin.Extensions.IoC.Tenancy.Middleware;

/// <summary>
/// Base class for <see cref="ICanGetTenantIdFromHttpContext"/> with method for getting logger.
/// </summary>
public abstract class TenantIdStrategy : ICanGetTenantIdFromHttpContext
{
    /// <summary>
    /// Optional synchronous implementation of getting <see cref="TenantId"/>.
    /// </summary>
    /// <param name="context">The <see cref="HttpContext"/>.</param>
    /// <param name="tenantId">The output <see cref="TenantId"/>.</param>
    /// <returns>False if <see cref="TenantId"/> not found, True if it was found.</returns>
    /// <exception cref="NotImplementedException">Thrown when forgetting to override <see cref="TryGet"/> and not providing a <see cref="GetAsync"/> override.</exception>
    public virtual bool TryGet(HttpContext context, [NotNullWhen(true)]out TenantId? tenantId)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public virtual Task<(bool Success, TenantId? TenantId)> GetAsync(HttpContext context)
    {
        var success = TryGet(context, out var tenantId);
        return Task.FromResult((success, tenantId));
    }

    /// <summary>
    /// Gets the <see cref="ILogger"/> from the <see cref="HttpContext.RequestServices"/>.
    /// </summary>
    /// <param name="context">The <see cref="HttpContext"/>.</param>
    /// <returns>The <see cref="ILogger"/>.</returns>
    protected ILogger GetLogger(HttpContext context) => context.RequestServices.GetService(typeof(ILogger<>).MakeGenericType(GetType())) as ILogger ?? NullLogger.Instance;
}
