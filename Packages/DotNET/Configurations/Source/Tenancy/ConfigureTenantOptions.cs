// Copyright (c) woksin-org. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Woksin.Extensions.Tenancy;

namespace Woksin.Extensions.Configurations.Tenancy;

/// <summary>
/// Represents an implementation of <see cref="IConfigureTenantOptions{TOptions,TTenant}"/>.
/// </summary>
/// <typeparam name="TOptions">The <see cref="Type"/> of the options.</typeparam>
/// <typeparam name="TTenant">The <see cref="Type"/> of the <see cref="ITenantInfo"/>.</typeparam>
public class ConfigureTenantOptions<TOptions, TTenant>(Action<TOptions, TTenant> configureOptions) : IConfigureTenantOptions<TOptions, TTenant>
    where TOptions : class, new()
    where TTenant : class, ITenantInfo, new()
{
    /// <inheritdoc />
    public void Configure(TOptions options, TTenant tenantInfo)
    {
        ArgumentNullException.ThrowIfNull(options);
        configureOptions?.Invoke(options, tenantInfo);
    }
}
