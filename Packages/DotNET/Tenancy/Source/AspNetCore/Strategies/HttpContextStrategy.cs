// Copyright (c) woksin-org. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Woksin.Extensions.Tenancy.Strategies;

namespace Woksin.Extensions.Tenancy.AspNetCore.Strategies;

/// <summary>
/// Represents a base implementation of <see cref="ITenantResolutionStrategy"/> that resolves from <see cref="HttpContext"/>.
/// </summary>
public abstract class HttpContextStrategy : ITenantResolutionStrategy
{
    const string CannotResolveFromNonHttpContextReason = "Cannot resolve from any context other than HttpContext";

    /// <inheritdoc />
    public bool CanResolveFromContext(object context, out string cannotResolveReason)
    {
        cannotResolveReason = string.Empty;
        if (context is HttpContext)
        {
            return true;
        }
        cannotResolveReason = CannotResolveFromNonHttpContextReason;

        return false;
    }

    /// <inheritdoc />
    public Task<string?> Resolve(object resolutionContext)
    {
        if (resolutionContext is not HttpContext context)
        {
            throw new ArgumentException("Expected resolution context to be HttpContext", nameof(resolutionContext));
        }
        return Resolve(context, context.RequestServices.GetService<ILoggerFactory>()?.CreateLogger(GetType()) ?? NullLogger.Instance);
    }
    
    /// <summary>
    /// Resolves the tenant identifier from the <see cref="HttpContext"/>.
    /// </summary>
    /// <param name="context">The <see cref="HttpContext"/>.</param>
    /// <param name="logger">The <see cref="ILogger"/>.</param>
    /// <returns>The resolved tenant identifier or null if couldn't be resolved.</returns>
    protected abstract Task<string?> Resolve(HttpContext context, ILogger logger);
}
