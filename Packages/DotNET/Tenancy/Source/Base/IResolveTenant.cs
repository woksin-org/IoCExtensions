// Copyright (c) woksin-org. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Woksin.Extensions.Tenancy.Strategies;

namespace Woksin.Extensions.Tenancy;

/// <summary>
/// Defines a system that can resolve the <see cref="ITenantContext"/>.
/// </summary>
public interface IResolveTenant
{
    /// <summary>
    /// Try resolve the tenant using the configured strategies.
    /// </summary>
    /// <param name="context">The resolution context.</param>
    /// <returns>The <see cref="ITenantContext"/>.</returns>
    Task<ITenantContext> Resolve(object context);
}
/// <summary>
/// Defines a system that can resolve the <see cref="ITenantContext{TTenantInfo}"/>.
/// </summary>
/// <typeparam name="TTenant">The <see cref="Type"/> of the <see cref="ITenantInfo"/>.</typeparam>
public interface IResolveTenant<TTenant>
    where TTenant : class, ITenantInfo, new()
{
    /// <summary>
    /// Gets the strategies used for resolving the <see cref="ITenantContext"/>.
    /// </summary>
    IEnumerable<ITenantResolutionStrategy> Strategies { get; }

    /// <summary>
    /// Try resolve the tenant using the configured strategies.
    /// </summary>
    /// <param name="context">The resolution context.</param>
    /// <returns>The <see cref="ITenantContext{TTenantInfo}"/>.</returns>
    Task<ITenantContext<TTenant>> Resolve(object context);
}
