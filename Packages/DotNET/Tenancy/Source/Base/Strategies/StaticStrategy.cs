// Copyright (c) woksin-org. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Woksin.Extensions.Tenancy.Strategies;

/// <summary>
/// Represents a static <see cref="ITenantResolutionStrategy"/>.
/// </summary>
/// <param name="identifier">The tenant identifier.</param>
public class StaticStrategy(string identifier) : ITenantResolutionStrategy
{
    public bool CanResolveFromContext(object context, out string cannotResolveReason)
    {
        cannotResolveReason = string.Empty;
        return true;
    }

    /// <inheritdoc />
    public Task<string?> Resolve(object resolutionContext)
        => Task.FromResult(identifier)!;
}
