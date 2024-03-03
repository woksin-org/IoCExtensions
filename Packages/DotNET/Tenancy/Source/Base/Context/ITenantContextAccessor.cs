// Copyright (c) woksin-org. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Woksin.Extensions.Tenancy.Context;

/// <summary>
/// Defines a system that can access the current <see cref="ITenantContext"/>.
/// </summary>
/// <typeparam name="TTenant">The <see cref="Type"/> of the <see cref="ITenantInfo"/>.</typeparam>
public interface ITenantContextAccessor<TTenant>
    where TTenant : class, ITenantInfo, new()
{
    /// <summary>
    /// Gets the current <see cref="ITenantContext"/>.
    /// </summary>
    ITenantContext<TTenant> CurrentTenant { get; set; }
    
    /// <summary>
    /// Throws an exception if the AsyncLocal tenant context is disabled. 
    /// </summary>
    /// <param name="accessor">The registered <see cref="ITenantContextAccessor"/>.</param>
    /// <param name="tenancyOptions">The configured <see cref="TenancyOptions{TTenant}"/>.</param>
    /// <exception cref="InvalidOperationException">The exception that is thrown.</exception>
    public static void ThrowIfDisabledTenantContext(ITenantContextAccessor accessor, TenancyOptions<TTenant> tenancyOptions)
    {
        if (accessor is StaticTenantContextAccessor<TTenant> && !tenancyOptions.IsUsingStaticTenant(out _))
        {
            throw new InvalidOperationException("Current tenant context cannot be resolved when AsyncLocal Tenant Context is disabled");
        }
    }
    /// <summary>
    /// Throws an exception if the AsyncLocal tenant context is disabled. 
    /// </summary>
    /// <param name="accessor">The registered <see cref="ITenantContextAccessor"/>.</param>
    /// <param name="tenancyOptions">The configured <see cref="TenancyOptions{TTenant}"/>.</param>
    /// <exception cref="InvalidOperationException">The exception that is thrown.</exception>
    public static void ThrowIfDisabledTenantContext(ITenantContextAccessor<TTenant> accessor, TenancyOptions<TTenant> tenancyOptions)
    {
        if (accessor is StaticTenantContextAccessor<TTenant> && !tenancyOptions.IsUsingStaticTenant(out _))
        {
            throw new InvalidOperationException("Current tenant context cannot be resolved when AsyncLocal Tenant Context is disabled");
        }
    }
}

/// <summary>
/// Defines a system that can access the current <see cref="ITenantContext"/>.
/// </summary>
public interface ITenantContextAccessor
{
    /// <summary>
    /// Gets the current <see cref="ITenantContext"/>.
    /// </summary>
    ITenantContext CurrentTenant { get; set; }
}
