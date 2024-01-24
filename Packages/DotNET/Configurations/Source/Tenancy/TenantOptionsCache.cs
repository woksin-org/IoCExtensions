// Copyright Finbuckle LLC, Andrew White, and Contributors.
// Refer to the solution LICENSE file for more information.

using System.Collections.Concurrent;
using Microsoft.Extensions.Options;
using Woksin.Extensions.Tenancy;

namespace Woksin.Extensions.Configurations.Tenancy;

/// <summary>
/// Represents an implementation of <see cref="IOptionsMonitor{TOptions}"/> for tenant options.
/// </summary>
/// <typeparam name="TOptions">The options.</typeparam>
/// <typeparam name="TTenantInfo">The tenant info.</typeparam>
public class TenantOptionsCache<TOptions, TTenantInfo> : IOptionsMonitorCache<TOptions>
    where TOptions : class
    where TTenantInfo : class, ITenantInfo, new()
{
    readonly ITenantContextAccessor<TTenantInfo> _multiTenantContextAccessor;
    readonly ConcurrentDictionary<string, IOptionsMonitorCache<TOptions>> _cache = new();

    /// <summary>
    /// Constructs a new instance of MultiTenantOptionsCache.
    /// </summary>
    /// <param name="multiTenantContextAccessor"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public TenantOptionsCache(ITenantContextAccessor<TTenantInfo> multiTenantContextAccessor)
    {
        _multiTenantContextAccessor = multiTenantContextAccessor;
    }

    /// <summary>
    /// Clears all cached options for the current tenant.
    /// </summary>
    public void Clear()
    {
        GetTenantCache().Clear();
    }

    /// <summary>
    /// Gets a named options instance for the current tenant, or adds a new instance created with createOptions.
    /// </summary>
    /// <param name="name">The options name.</param>
    /// <param name="createOptions">The factory function for creating the options instance.</param>
    /// <returns>The existing or new options instance.</returns>
    public TOptions GetOrAdd(string? name, Func<TOptions> createOptions)
    {
        ArgumentNullException.ThrowIfNull(createOptions);
        name ??= Options.DefaultName;
        return GetTenantCache().GetOrAdd(name, createOptions);
    }

    /// <summary>
    /// Tries to adds a new option to the cache for the current tenant.
    /// </summary>
    /// <param name="name">The options name.</param>
    /// <param name="options">The options instance.</param>
    /// <returns>True if the options was added to the cache for the current tenant.</returns>
    public bool TryAdd(string? name, TOptions options)
    {
        name ??= Options.DefaultName;
        return GetTenantCache().TryAdd(name, options);
    }

    /// <summary>
    /// Try to remove an options instance for the current tenant.
    /// </summary>
    /// <param name="name">The options name.</param>
    /// <returns>True if the options was removed from the cache for the current tenant.</returns>
    public bool TryRemove(string? name)
    {
        name ??= Options.DefaultName;
        return GetTenantCache().TryRemove(name);
    }

    string GetTenantId()
    {
        var tenantId = "";
        if (_multiTenantContextAccessor.CurrentTenant.Resolved(out var tenantInfo, out _))
        {
            tenantId = tenantInfo.Id;
        }
        return tenantId;
    }

    IOptionsMonitorCache<TOptions> GetTenantCache() => _cache.GetOrAdd(GetTenantId(), new OptionsCache<TOptions>());
}
