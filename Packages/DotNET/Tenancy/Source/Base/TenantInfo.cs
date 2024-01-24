// Copyright (c) woksin-org. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Woksin.Extensions.Tenancy;

/// <summary>
/// Represents the basic implementation of <see cref="ITenantInfo"/>.
/// </summary>
public class TenantInfo : ITenantInfo
{
    /// <inheritdoc />
    public string Id { get; set; } = null!;

    /// <inheritdoc />
    public string? Name { get; set; }
}
