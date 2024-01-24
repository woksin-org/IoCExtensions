// Copyright (c) woksin-org. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Woksin.Extensions.Tenancy;

/// <summary>
/// Represents the async local implementation of <see cref="ITenantContextAccessor{TTenant}"/> and <see cref="ITenantContextAccessor"/>.
/// </summary>
/// <typeparam name="TTenant">The <see cref="Type"/> of the <see cref="ITenantInfo"/>.</typeparam>
public class AsyncLocalTenantContextAccessor<TTenant> : ITenantContextAccessor<TTenant>, ITenantContextAccessor
    where TTenant : class, ITenantInfo, new()
{
    static readonly AsyncLocal<ITenantContext<TTenant>> _local = new();

    public AsyncLocalTenantContextAccessor()
    {
        CurrentTenant = TenantContext<TTenant>.Unresolved();
    }

    /// <inheritdoc />
    public ITenantContext<TTenant> CurrentTenant
    {
        get => _local.Value!;

        set => _local.Value = value ?? throw new ArgumentNullException(nameof(value));
    }

    /// <inheritdoc />
    ITenantContext ITenantContextAccessor.CurrentTenant
    {
        get => (CurrentTenant as ITenantContext)!;
        set => CurrentTenant = value as ITenantContext<TTenant> ?? CurrentTenant;
    }
}
