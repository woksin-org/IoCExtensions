// Copyright (c) woksin-org. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Woksin.Extensions.Tenancy;

/// <summary>
/// Represents the options for tenancy system.
/// </summary>
/// <typeparam name="TTenant">The <see cref="Type"/> of the <see cref="ITenantInfo"/>.</typeparam>
public class TenancyOptions<TTenant>
    where TTenant : class, ITenantInfo, new()
{
    public IList<TTenant> Tenants { get; set; } = new List<TTenant>();

    public bool Strict { get; set; }

    public IList<string> Ignored { get; set; } = new List<string>();
}
