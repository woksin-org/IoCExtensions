// Copyright (c) woksin-org. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Woksin.Extensions.IoC.Tenancy.Middleware;

/// <summary>
/// Defines the tenant id filter options.
/// </summary>
public interface ITenantIdFilterOptions
{
    /// <summary>
    /// Gets the included tenant ids.
    /// </summary>
    public IReadOnlySet<TenantId> ToInclude { get; }

    /// <summary>
    /// Gets the excluded tenant ids.
    /// </summary>
    public IReadOnlySet<TenantId> ToExclude { get; }
}
