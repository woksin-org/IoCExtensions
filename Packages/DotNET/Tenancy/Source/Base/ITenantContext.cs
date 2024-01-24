// Copyright (c) woksin-org. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Diagnostics.CodeAnalysis;
using Woksin.Extensions.Tenancy.Strategies;

namespace Woksin.Extensions.Tenancy;

/// <summary>
/// Defines the tenant context, mostly used to represent the tenant that's currently in use.
/// </summary>
public interface ITenantContext
{
    /// <summary>
    /// The resolved tenant.
    /// </summary>
    ITenantInfo? Tenant { get; }

    /// <summary>
    /// The strategy used to resolve the tenant.
    /// </summary>
    StrategyInfo? Strategy { get; }

    /// <summary>
    /// Whether the tenant has been resolved or not.
    /// </summary>
    /// <param name="tenantInfo">The outputted <see cref="ITenantInfo"/>.</param>
    /// <param name="strategyInfo">The outputted <see cref="StrategyInfo"/>.</param>
    bool Resolved([NotNullWhen(true)] out ITenantInfo? tenantInfo, [NotNullWhen(true)] out StrategyInfo? strategyInfo)
    {
        tenantInfo = null;
        strategyInfo = null;
        if (Tenant is null)
        {
            return false;
        }
        tenantInfo = Tenant!;
        strategyInfo = Strategy!;
        return true;
    }
}

/// <summary>
/// Defines the tenant context, mostly used to represent the tenant that's currently in use.
/// </summary>
/// <typeparam name="TTenantInfo">The <see cref="Type"/> of the <see cref="ITenantInfo"/>.</typeparam>
public interface ITenantContext<TTenantInfo>
    where TTenantInfo : class, ITenantInfo, new()
{
    /// <summary>
    /// Whether the tenant has been resolved or not.
    /// </summary>
    /// <param name="tenantInfo">The outputted <see cref="ITenantInfo"/>.</param>
    /// <param name="strategyInfo">The outputted <see cref="StrategyInfo"/>.</param>
    bool Resolved([NotNullWhen(true)] out TTenantInfo? tenantInfo, [NotNullWhen(true)] out StrategyInfo? strategyInfo);
}
