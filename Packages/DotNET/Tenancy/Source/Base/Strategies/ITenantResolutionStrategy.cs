// Copyright (c) woksin-org. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Woksin.Extensions.Tenancy.Strategies;

/// <summary>
/// Defines a system that can resolve a tenant identifier from a context.
/// </summary>
public interface ITenantResolutionStrategy
{
    /// <summary>
    /// Resolves the tenant identifier from a given context.
    /// </summary>
    /// <param name="resolutionContext">The context used to resolve the tenant identifier.</param>
    /// <returns>The resolved tenant identifier or null.</returns>
    Task<string?> Resolve(object resolutionContext);
}
