// Copyright (c) woksin-org. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Woksin.Extensions.Tenancy.Context;

namespace Woksin.Extensions.Tenancy;

/// <summary>
/// Defines the basic details of a tenant.
/// </summary>
/// <remarks>This interface can be derived and expanded to include additional information and configuration for the tenants.</remarks>
public interface ITenantInfo
{
    /// <summary>
    /// Gets the unique identifier for the tenant.
    /// </summary>
    string Id { get; internal set; }

    /// <summary>
    /// Gets the friendly name of the tenant.
    /// </summary>
    string? Name { get; }
}
