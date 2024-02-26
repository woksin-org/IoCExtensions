// Copyright (c) woksin-org. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Woksin.Extensions.Tenancy.Strategies;

/// <summary>
/// Defines a system that can resolve a tenant identifier from a context.
/// </summary>
public interface ITenantResolutionStrategy
{
    /// <summary>
    /// Check if tenant id can be attempted resolved from the given context.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="cannotResolveReason">The reason why it cannot be resolved using this strategy.</param>
    /// <returns>True if can resolve, false if not.</returns>
    bool CanResolveFromContext(object context, out string cannotResolveReason);

    /// <summary>
    /// Resolves the tenant identifier from a given context.
    /// </summary>
    /// <param name="resolutionContext">The context used to resolve the tenant identifier.</param>
    /// <returns>The resolved tenant identifier or null.</returns>
    Task<string?> Resolve(object resolutionContext);
}
