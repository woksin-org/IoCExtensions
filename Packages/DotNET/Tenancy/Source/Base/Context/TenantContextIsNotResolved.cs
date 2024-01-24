// Copyright (c) woksin-org. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Woksin.Extensions.Tenancy.Context;

/// <summary>
/// Exception that gets thrown when the current tenant context is not set.
/// </summary>
public class TenantContextIsNotResolved : Exception
{
    public TenantContextIsNotResolved()
        : base("Tenant context is not resolved")
    {
    }
    public TenantContextIsNotResolved(string message)
        : base($"Tenant context is not resolved. {message}")
    {
    }
}
