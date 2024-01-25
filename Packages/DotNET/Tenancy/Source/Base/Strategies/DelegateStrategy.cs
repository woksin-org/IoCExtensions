// Copyright (c) woksin-org. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Woksin.Extensions.Tenancy.Strategies;

/// <summary>
/// Represents a delegate <see cref="ITenantResolutionStrategy"/>;
/// </summary>
/// <param name="doStrategy">The callback representing the <see cref="ITenantResolutionStrategy"/> to perform.</param>
/// <param name="canResolve">The callback to check whether can be resolved from context.</param>
public class DelegateStrategy(Func<object, Task<string?>> doStrategy, Func<object, (bool, string)>? canResolve = null) : ITenantResolutionStrategy
{
    static (bool, string) DefaultCheck(object o) => (true, string.Empty);

    readonly Func<object, (bool, string)> _checkCanResolve = canResolve ?? DefaultCheck;

    /// <inheritdoc />
    public bool CanResolveFromContext(object context, out string cannotResolveReason)
    {
        (var canResolve, cannotResolveReason) = _checkCanResolve(context);
        return canResolve;
    }

    /// <inheritdoc />
    public Task<string?> Resolve(object resolutionContext)
        => doStrategy(resolutionContext);
}
