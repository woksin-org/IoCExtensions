// Copyright (c) woksin-org. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.ComponentModel;

namespace Woksin.Extensions.IoC.Tenancy;

/// <summary>
/// Represents the unique identifier for a tenant.
/// </summary>
/// <param name="Value"></param>
public record TenantId(string Value)
{
    static TenantId()
    {
        TypeDescriptor.AddAttributes(typeof(TenantId), new TypeConverterAttribute(typeof(TenantIdTypeConverter)));
    }

    public static implicit operator string(TenantId tenantId) => tenantId.Value;
    public static implicit operator TenantId(string tenantId) => new(tenantId);

    /// <inheritdoc />
    public override string ToString() => Value;

    /// <summary>
    /// Create a <see cref="TenantId"/> from a string.
    /// </summary>
    /// <param name="tenantId">The tenant id string.</param>
    /// <returns>The <see cref="TenantId"/>.</returns>
    public static TenantId FromString(string tenantId) => new(tenantId);
}
