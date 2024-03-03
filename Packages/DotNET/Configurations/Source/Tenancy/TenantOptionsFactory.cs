// Copyright (c) woksin-org. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Woksin.Extensions.Tenancy;
using Woksin.Extensions.Tenancy.Context;

namespace Woksin.Extensions.Configurations.Tenancy;

/// <summary>
/// Represents an implementation of <see cref="ConfigurationsExtensionOptionsFactory{TOptions}"/> for tenant configurations.
/// </summary>
/// <typeparam name="TOptions">The type of options being requested.</typeparam>
/// <typeparam name="TTenant">The type of the tenant info.</typeparam>
public class TenantOptionsFactory<TOptions, TTenant> : BaseTenantOptionsFactory<TOptions, TTenant>
    where TOptions : class, new()
    where TTenant : class, ITenantInfo, new()
{
    readonly ITenantContextAccessor<TTenant> _multiTenantContextAccessor;

    public TenantOptionsFactory(
        IConfiguration configuration,
        ConfigurationPrefix configurationPrefix,
        IEnumerable<ConfigurationObjectDefinition<TOptions>> configurationDefinitions,
        IEnumerable<IConfigureOptions<TOptions>> configureOptions,
        IEnumerable<IPostConfigureOptions<TOptions>> postConfigureOptions,
        IEnumerable<IValidateOptions<TOptions>> validations,
        IEnumerable<IConfigureTenantOptions<TOptions, TTenant>> configureTenantOptions,
        ITenantContextAccessor<TTenant> multiTenantContextAccessor)
        : base(configuration, configurationPrefix, configurationDefinitions, configureOptions, postConfigureOptions, validations, configureTenantOptions)
    {
        _multiTenantContextAccessor = multiTenantContextAccessor;
    }

    protected override ITenantContext<TTenant> GetTenantContext() => _multiTenantContextAccessor.CurrentTenant;
}

/// <summary>
/// Represents an implementation of <see cref="BaseTenantOptionsFactory{TOptions, TTenant}"/> for tenant configurations.
/// </summary>
/// <typeparam name="TOptions">The type of options being requested.</typeparam>
/// <typeparam name="TTenant">The type of the tenant info.</typeparam>
public class StaticTenantOptionsFactory<TOptions, TTenant> : BaseTenantOptionsFactory<TOptions, TTenant>
    where TOptions : class, new()
    where TTenant : class, ITenantInfo, new()
{
    readonly ITenantContext<TTenant> _tenantContext;

    public StaticTenantOptionsFactory(
        IConfiguration configuration,
        ConfigurationPrefix configurationPrefix,
        IEnumerable<ConfigurationObjectDefinition<TOptions>> configurationDefinitions,
        IEnumerable<IConfigureOptions<TOptions>> configureOptions,
        IEnumerable<IPostConfigureOptions<TOptions>> postConfigureOptions,
        IEnumerable<IValidateOptions<TOptions>> validations,
        IEnumerable<IConfigureTenantOptions<TOptions, TTenant>> configureTenantOptions,
        ITenantContext<TTenant> tenantContext)
        : base(configuration, configurationPrefix, configurationDefinitions, configureOptions, postConfigureOptions, validations, configureTenantOptions)
    {
        _tenantContext = tenantContext;
    }

    protected override ITenantContext<TTenant> GetTenantContext() => _tenantContext;
}

public abstract class BaseTenantOptionsFactory<TOptions, TTenant> : ConfigurationsExtensionOptionsFactory<TOptions>
    where TOptions : class, new()
    where TTenant : class, ITenantInfo, new()
{
    readonly IEnumerable<IConfigureTenantOptions<TOptions, TTenant>> _configureTenantOptions;

    TTenant? _tenantInfo;

    protected BaseTenantOptionsFactory(
        IConfiguration configuration,
        ConfigurationPrefix configurationPrefix,
        IEnumerable<ConfigurationObjectDefinition<TOptions>> configurationDefinitions,
        IEnumerable<IConfigureOptions<TOptions>> configureOptions,
        IEnumerable<IPostConfigureOptions<TOptions>> postConfigureOptions,
        IEnumerable<IValidateOptions<TOptions>> validations,
        IEnumerable<IConfigureTenantOptions<TOptions, TTenant>> configureTenantOptions)
        : base(configuration, configurationPrefix, configurationDefinitions, configureOptions, postConfigureOptions, validations)
    {
        _configureTenantOptions = configureTenantOptions;
    }

    protected abstract ITenantContext<TTenant> GetTenantContext();

    TTenant GetTenantInfo()
    {
        if (!GetTenantContext().Resolved(out var tenantInfo, out _))
        {
            throw new CannotResolveTenantConfigurationWhenTenantContextIsNotResolved(typeof(TOptions));
        }
        return tenantInfo;
    }
    
    protected override TOptions CreateInstance(string name)
    {
        _tenantInfo = GetTenantInfo();
        var options = base.CreateInstance(name);
        foreach (var configure in _configureTenantOptions)
        {
            configure.Configure(options, _tenantInfo);
        }
        return options;
    }
    
    protected override string GetConfigurationPath(ConfigurationObjectDefinition<TOptions> definition)
        => GetConfigurationPathWithPrefix(ConfigurationPath.Combine(_tenantInfo!.Id, definition.ConfigurationPath));
}
