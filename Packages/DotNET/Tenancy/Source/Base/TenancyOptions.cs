// Copyright (c) woksin-org. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Diagnostics.CodeAnalysis;
using Woksin.Extensions.Tenancy.Context;
using Woksin.Extensions.Tenancy.Strategies;

namespace Woksin.Extensions.Tenancy;

/// <summary>
/// Represents the options for tenancy system.
/// </summary>
/// <typeparam name="TTenant">The <see cref="Type"/> of the <see cref="ITenantInfo"/>.</typeparam>
public class TenancyOptions<TTenant>
    where TTenant : class, ITenantInfo, new()
{
    /// <summary>
    /// Gets ir sets tge configured tenants.
    /// </summary>
    public IList<TTenant> Tenants { get; set; } = new List<TTenant>();

    /// <summary>
    /// Gets or sets the value indicating whether only configured tenants can be resolved.
    /// </summary>
    public bool Strict { get; set; }

    /// <summary>
    /// Gets or sets the list of tenant ids to ignore.
    /// </summary>
    public IList<string> Ignored { get; set; } = new List<string>();
    
    /// <summary>
    /// Gets or sets the optional static tenant id. When this is configured the tenancy system will only use this static tenant id and never try to resolve tenant.
    /// The application will constantly be in context of this tenant.
    /// </summary>
    /// <remarks>This is different from using the <see cref="StaticStrategy"/> because it acts as both a fallback tenant resolution strategy and does not put the entire application in the context of a tenant.</remarks>
    public string? StaticTenantId { get; set; }

    public bool IsUsingStaticTenant([NotNullWhen(true)]out string? staticTenantId)
    {
        staticTenantId = string.IsNullOrEmpty(StaticTenantId) ? null : StaticTenantId;
        return !string.IsNullOrEmpty(staticTenantId);
    }

    /// <summary>
    /// Tries to get the resolved <see cref="ITenantContext{TTenant}"/> from the <see cref="TenancyOptions{TTenant}"/>.
    /// </summary>
    /// <param name="tenantId">The tenant identifier.</param>
    /// <param name="strategy">The optional strategy that resolved the tenant identifier.</param>
    /// <param name="tenantContext">The outputted <see cref="ITenantContext{TTenant}"/>.</param>
    public (bool IsResolved, bool IsResolvedFromConfig) TryGetTenantContext(string tenantId, ITenantResolutionStrategy? strategy, out ITenantContext<TTenant> tenantContext)
    {
        var configuredTenant = Tenants.FirstOrDefault(tenant => tenant.Id.Equals(tenantId, StringComparison.OrdinalIgnoreCase));
        var strategyInfo = strategy is not null
            ? new StrategyInfo(strategy.GetType(), strategy)
            : null; 
        if (configuredTenant is not null)
        {
            tenantContext = TenantContext<TTenant>.Resolved(configuredTenant, strategyInfo);
            return (true, true);
        }

        if (Strict)
        {
            tenantContext = TenantContext<TTenant>.Unresolved();
            return (false, false);
        }
        configuredTenant = new TTenant
        {
            Id = tenantId
        };
        tenantContext = TenantContext<TTenant>.Resolved(configuredTenant, strategyInfo);
        return (true, false);
    }

    /// <summary>
    /// Tries to get the resolved <see cref="ITenantContext{TTenant}"/> from the <see cref="TenancyOptions{TTenant}"/>.
    /// </summary>
    /// <param name="tenantId">The tenant identifier.</param>
    /// <param name="tenantContext">The outputted <see cref="ITenantContext{TTenant}"/>.</param>
    public (bool IsResolved, bool IsResolvedFromConfig) TryGetTenantContext(string tenantId, out ITenantContext<TTenant> tenantContext)
        => TryGetTenantContext(tenantId, null, out tenantContext);
}
