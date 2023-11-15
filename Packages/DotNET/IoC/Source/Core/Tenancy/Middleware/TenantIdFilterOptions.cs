// Copyright (c) woksin-org. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Woksin.Extensions.IoC.Tenancy.Middleware;

/// <summary>
/// Represents the base implementation of <see cref="ITenantIdFilterOptions"/>.
/// </summary>
[DisableAutoRegistration]
public class TenantIdFilterOptions : ITenantIdFilterOptions
{
    /// <summary>
    /// Gets or sets the included tenant ids.
    /// </summary>
    public IList<TenantId> Included { get; set; } = new List<TenantId>();

    /// <summary>
    /// Gets or sets the included tenant ids.
    /// </summary>
    public IList<TenantId> Excluded { get; set; } = new List<TenantId>();

    /// <inheritdoc />
    public IReadOnlySet<TenantId> ToInclude => Included.ToHashSet();

    /// <inheritdoc />
    public IReadOnlySet<TenantId> ToExclude => Excluded.ToHashSet();
}
