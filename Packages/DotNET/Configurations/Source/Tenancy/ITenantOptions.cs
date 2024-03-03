// Copyright (c) woksin-org. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Woksin.Extensions.Tenancy;
using Woksin.Extensions.Tenancy.Context;

namespace Woksin.Extensions.Configurations.Tenancy;

/// <summary>
/// Represents a system for getting options for tenant configurations without relying on the <see cref="TenantContextAccessor{TTenant}"/>.
/// </summary>
public interface ITenantOptions
{
    public IOptions<TOptions> OptionsFor<TOptions>(string tenantId)
        where TOptions : class, new();
    public IOptionsSnapshot<TOptions> OptionsSnapshotFor<TOptions>(string tenantId)
        where TOptions : class, new();
    public IOptionsMonitor<TOptions> OptionsMonitorFor<TOptions>(string tenantId)
        where TOptions : class, new();
}

/// <summary>
/// Represents a system for getting options for tenant configurations without relying on the <see cref="TenantContextAccessor{TTenant}"/>.
/// </summary>
/// <typeparam name="TTenant">The <see cref="Type"/> of <see cref="ITenantInfo"/>.</typeparam>
public interface ITenantOptions<TTenant>
    where TTenant : class, ITenantInfo, new()
{
    public IOptions<TOptions> OptionsFor<TOptions>(ITenantContext<TTenant> tenant)
        where TOptions : class, new();
    public IOptionsSnapshot<TOptions> OptionsSnapshotFor<TOptions>(ITenantContext<TTenant> tenant)
        where TOptions : class, new();
    
    public IOptionsMonitor<TOptions> OptionsMonitorFor<TOptions>(ITenantContext<TTenant> tenant)
        where TOptions : class, new();
}

/// <summary>
/// Represents an implementation of <see cref="ITenantOptions{TTenant}"/> keeping an internal state of the <see cref="IOptions{TOptions}"/>;
/// </summary>
/// <typeparam name="TTenant"></typeparam>
public class TenantOptions<TTenant> : ITenantOptions<TTenant>, ITenantOptions
    where TTenant : class, ITenantInfo, new()
{
    readonly IOptionsMonitor<TenancyOptions<TTenant>> _options;
    readonly IServiceProvider _serviceProvider;
    readonly ConcurrentDictionary<Type, ConcurrentDictionary<string, object>> _optionsPerTenantPerType = new();
    readonly ConcurrentDictionary<Type, ConcurrentDictionary<string, object>> _optionsMonitorsPerTenantPerType = new();

    public TenantOptions(IOptionsMonitor<TenancyOptions<TTenant>> options, IServiceProvider serviceProvider)
    {
        _options = options;
        _serviceProvider = serviceProvider;
    }
    
    public IOptions<TOptions> OptionsFor<TOptions>(string tenantId)
        where TOptions : class, new()
    {
        _options.CurrentValue.TryGetTenantContext(tenantId, out var tenantContext);
        return OptionsFor<TOptions>(tenantContext);
    }

    public IOptions<TOptions> OptionsFor<TOptions>(ITenantContext<TTenant> tenant)
        where TOptions : class, new()
    {
        ThrowIfTenantNotResolved<TOptions>(tenant, out var tenantInfo);
        var optionsPerTenant = _optionsPerTenantPerType.GetOrAdd(typeof(TOptions), new ConcurrentDictionary<string, object>());
        return (IOptions<TOptions>)optionsPerTenant.GetOrAdd(tenantInfo.Id, _ => CreateTenantOptionsManager<TOptions>(tenant));
    }

    public IOptionsSnapshot<TOptions> OptionsSnapshotFor<TOptions>(string tenantId)
        where TOptions : class, new()
    {
        _options.CurrentValue.TryGetTenantContext(tenantId, out var tenantContext);
        return OptionsSnapshotFor<TOptions>(tenantContext);
    }

    public IOptionsSnapshot<TOptions> OptionsSnapshotFor<TOptions>(ITenantContext<TTenant> tenant)
        where TOptions : class, new()
    {
        ThrowIfTenantNotResolved<TOptions>(tenant, out _);
        return CreateTenantOptionsManager<TOptions>(tenant);
    }

    public IOptionsMonitor<TOptions> OptionsMonitorFor<TOptions>(string tenantId) where TOptions : class, new()
    {
        _options.CurrentValue.TryGetTenantContext(tenantId, out var tenantContext);
        return OptionsMonitorFor<TOptions>(tenantContext);
    }

    public IOptionsMonitor<TOptions> OptionsMonitorFor<TOptions>(ITenantContext<TTenant> tenant) where TOptions : class, new()
    {
        ThrowIfTenantNotResolved<TOptions>(tenant, out var tenantInfo);
        var optionsMonitorsPerTenant = _optionsMonitorsPerTenantPerType.GetOrAdd(typeof(TOptions), new ConcurrentDictionary<string, object>());
        return (IOptionsMonitor<TOptions>)optionsMonitorsPerTenant.GetOrAdd(tenantInfo.Id, _ => new OptionsMonitor<TOptions>(
            CreateOptionsFactory<TOptions>(tenant),
            _serviceProvider.GetRequiredService<IEnumerable<IOptionsChangeTokenSource<TOptions>>>(),
            new OptionsCache<TOptions>()));
    }

    StaticTenantOptionsFactory<TOptions, TTenant> CreateOptionsFactory<TOptions>(ITenantContext<TTenant> tenant)
        where TOptions : class, new() =>
        ActivatorUtilities.CreateInstance<StaticTenantOptionsFactory<TOptions, TTenant>>(_serviceProvider, tenant);

    TenantOptionsManager<TOptions> CreateTenantOptionsManager<TOptions>(ITenantContext<TTenant> tenant) where TOptions : class, new()
        => new(CreateOptionsFactory<TOptions>(tenant), new OptionsCache<TOptions>());
    
    void ThrowIfTenantNotResolved<TOptions>(ITenantContext<TTenant> tenant, [NotNull]out TTenant tenantInfo)
        where TOptions : class, new()
    {
        if (!tenant.Resolved(out tenantInfo!, out _))
        {
            throw new CannotResolveTenantConfigurationWhenTenantContextIsNotResolved(typeof(TOptions));
        }
    }
}
