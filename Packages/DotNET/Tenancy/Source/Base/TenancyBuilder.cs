// Copyright (c) woksin-org. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Woksin.Extensions.Tenancy.Context;
using Woksin.Extensions.Tenancy.Strategies;

namespace Woksin.Extensions.Tenancy;

public class TenancyBuilder<TTenant>
    where TTenant : class, ITenantInfo, new()
{
    readonly IServiceCollection _services;

    public TenancyBuilder(IServiceCollection services)
    {
        _services = services;
        _services.TryAddScoped<IResolveTenant<TTenant>, TenantResolver<TTenant>>();
        _services.TryAddScoped<IResolveTenant>(sp => (IResolveTenant)sp.GetRequiredService<IResolveTenant<TTenant>>());

        _services.TryAddScoped<ITenantContext<TTenant>>(sp =>
            sp.GetRequiredService<ITenantContextAccessor<TTenant>>().CurrentTenant);

        _services.TryAddScoped<TTenant>(sp =>
        {
            var accessor = sp.GetRequiredService<ITenantContextAccessor<TTenant>>();
            if (!accessor.CurrentTenant.Resolved(out var tenantInfo, out _))
            {
                throw new TenantContextIsNotResolved($"Cannot resolve {typeof(TTenant)}");
            }
            return tenantInfo;
        });
        _services.TryAddScoped<ITenantInfo>(sp => sp.GetService<TTenant>()!);
        _services.TryAddSingleton<ITenantContextAccessor<TTenant>, TenantContextAccessor<TTenant>>();
        _services.TryAddSingleton<ITenantContextAccessor>(sp =>
            (ITenantContextAccessor)sp.GetRequiredService<ITenantContextAccessor<TTenant>>());
        _services.TryAddSingleton<IPerformActionInTenantContext<TTenant>, ActionInTenantContextPerformer<TTenant>>();
        _services.TryAddSingleton<IPerformActionInTenantContext>(sp =>
            (IPerformActionInTenantContext)sp.GetRequiredService<IPerformActionInTenantContext<TTenant>>());
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
