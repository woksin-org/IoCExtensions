// Copyright (c) woksin-org. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Woksin.Extensions.Tenancy.Strategies;

/// <summary>
/// Represents a delegate <see cref="ITenantResolutionStrategy"/>;
/// </summary>
/// <param name="doStrategy">The callback representing the <see cref="ITenantResolutionStrategy"/> to perform.</param>
public class DelegateStrategy(Func<object, Task<string?>> doStrategy) : ITenantResolutionStrategy
{
    public Task<string?> Resolve(object resolutionContext)
        => doStrategy(resolutionContext);
}
