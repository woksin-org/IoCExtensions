// Copyright (c) woksin-org. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Woksin.Extensions.Tenancy;

namespace Woksin.Extensions.Configurations.Tenancy;

/// <summary>
/// Configures options per-tenant.
/// </summary>
/// <typeparam name="TOptions">Options type being configured.</typeparam>
/// <typeparam name="TTenant">A type implementing ITenantInfo.</typeparam>
public interface IConfigureTenantOptions<in TOptions, in TTenant>
    where TOptions : class
    where TTenant : class, ITenantInfo, new()
{
    /// <summary>
    /// Invoked to configure per-tenant options.
    /// </summary>
    /// <param name="options">The options class instance to be configured.</param>
    /// <param name="tenantInfo">The TTenantInfo instance for the options being configured.</param>
    void Configure(TOptions options, TTenant tenantInfo);
}
