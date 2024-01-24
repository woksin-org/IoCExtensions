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
