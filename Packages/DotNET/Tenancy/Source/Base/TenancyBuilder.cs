// Copyright (c) woksin-org. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Woksin.Extensions.Tenancy.Context;
using Woksin.Extensions.Tenancy.Strategies;

namespace Woksin.Extensions.Tenancy;

public class TenancyBuilder<TTenant>
    where TTenant : class, ITenantInfo, new()
{
    readonly IServiceCollection _services;
    bool _disableAsyncLocalTenantContext;

    public TenancyBuilder(IServiceCollection services)
    {
        _services = services;
        // Simply configure the TenancyOptions so that it is at least registered;
        Configure(_ => { });
        _services.TryAddTransient<IResolveTenant<TTenant>, TenantResolver<TTenant>>();
        _services.TryAddTransient<IResolveTenant>(sp => (IResolveTenant)sp.GetRequiredService<IResolveTenant<TTenant>>());
        _services.TryAddScoped<ITenantContext<TTenant>>(sp =>
        {
            var accessor = sp.GetRequiredService<ITenantContextAccessor<TTenant>>();
            ITenantContextAccessor<TTenant>.ThrowIfDisabledTenantContext(accessor, sp.GetRequiredService<IOptionsMonitor<TenancyOptions<TTenant>>>().CurrentValue);
            return accessor.CurrentTenant;
        });

        _services.TryAddScoped<TTenant>(sp =>
        {
            var tenantContext = sp.GetRequiredService<ITenantContext<TTenant>>();
            if (!tenantContext.Resolved(out var tenantInfo, out _))
            {
                throw new TenantContextIsNotResolved($"Cannot resolve {typeof(TTenant)}");
            }
            return tenantInfo;
        });
        _services.TryAddScoped<ITenantInfo>(sp => sp.GetService<TTenant>()!);
        if (_disableAsyncLocalTenantContext)
        {
            _services.TryAddSingleton<ITenantContextAccessor<TTenant>, StaticTenantContextAccessor<TTenant>>();
        }
        else
        {
            _services.TryAddSingleton<ITenantContextAccessor<TTenant>, TenantContextAccessor<TTenant>>();
        }
        _services.TryAddSingleton<ITenantContextAccessor>(sp =>
            (ITenantContextAccessor)sp.GetRequiredService<ITenantContextAccessor<TTenant>>());
        _services.TryAddTransient<IPerformActionInTenantContext<TTenant>, ActionInTenantContextPerformer<TTenant>>();
        _services.TryAddTransient<IPerformActionInTenantContext>(sp =>
            (IPerformActionInTenantContext)sp.GetRequiredService<IPerformActionInTenantContext<TTenant>>());
    }

    /// <summary>
    /// Disables the option to use an <see cref="AsyncLocal{T}"/> <see cref="ITenantContext{TTenantInfo}"/> tenant context using the <see cref="TenantContextAccessor{TTenant}"/> implementation of <see cref="ITenantContextAccessor{TTenant}"/>.
    /// The <see cref="StaticTenantContextAccessor{TTenant}"/> will now be used instead of <see cref="TenantContextAccessor{TTenant}"/>.
    /// </summary>
    /// <remarks><see cref="StaticTenantContextAccessor{TTenant}"/> will not care for any <see cref="ITenantContext"/> state and will simply always return the 'unresolved' <see cref="ITenantContext"/>.</remarks>
    public TenancyBuilder<TTenant> DisableAsyncLocalTenantContext()
    {
        _disableAsyncLocalTenantContext = true;
        return this;
    }
    
    public TenancyBuilder<TTenant> WithTenantInfo(TTenant tenantInfo)
    {
        _services.Configure((TenancyOptions<TTenant> op) => op.Tenants.Add(tenantInfo));
        return this;
    }
    public TenancyBuilder<TTenant> IgnoreTenant(string tenantId)
    {
        _services.Configure((TenancyOptions<TTenant> op) => op.Ignored.Add(tenantId));
        return this;
    }
    public TenancyBuilder<TTenant> Configure(Action<TenancyOptions<TTenant>> configure)
    {
        _services.Configure(configure);
        return this;
    }

    public TenancyBuilder<TTenant> WithStrategy(ITenantResolutionStrategy strategy)
    {
        _services.AddSingleton(strategy);
        return this;
    }

    public TenancyBuilder<TTenant> WithStaticStrategy(string tenantId)
    {
        _services.AddSingleton<ITenantResolutionStrategy>(new StaticStrategy(tenantId));
        return this;
    }

    public TenancyBuilder<TTenant> WithStrategy(Func<object, Task<string?>> delegateStrategy)
    {
        _services.AddSingleton<ITenantResolutionStrategy>(new DelegateStrategy(delegateStrategy));
        return this;
    }

    public TenancyBuilder<TTenant> WithStrategy<TStrategy>(ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
        where TStrategy : ITenantResolutionStrategy
        => WithStrategy(typeof(TStrategy), serviceLifetime);

    public TenancyBuilder<TTenant> WithStrategy(Type strategyType, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
    {
        _services.Add(new ServiceDescriptor(typeof(ITenantResolutionStrategy), strategyType, serviceLifetime));
        return this;
    }
}
