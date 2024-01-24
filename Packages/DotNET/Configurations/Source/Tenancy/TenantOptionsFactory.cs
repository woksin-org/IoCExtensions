// Copyright (c) woksin-org. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Woksin.Extensions.Tenancy;

namespace Woksin.Extensions.Configurations.Tenancy;

/// <summary>
/// Represents an implementation of <see cref="ConfigurationsExtensionOptionsFactory{TOptions}"/> for tenant configurations.
/// </summary>
/// <typeparam name="TOptions">The type of options being requested.</typeparam>
/// <typeparam name="TTenantInfo">The type of the tenant info.</typeparam>
public class TenantOptionsFactory<TOptions, TTenantInfo> : ConfigurationsExtensionOptionsFactory<TOptions>
    where TOptions : class, new()
    where TTenantInfo : class, ITenantInfo, new()
{
    readonly IConfigureTenantOptions<TOptions, TTenantInfo>[] _configureTenantOptions;
    readonly ITenantContextAccessor<TTenantInfo> _multiTenantContextAccessor;
    TTenantInfo _tenantInfo = null!;

    public TenantOptionsFactory(
        IConfiguration configuration,
        ConfigurationPrefix configurationPrefix,
        IEnumerable<ConfigurationObjectDefinition<TOptions>> configurationDefinitions,
        IEnumerable<IConfigureOptions<TOptions>> configureOptions,
        IEnumerable<IPostConfigureOptions<TOptions>> postConfigureOptions,
        IEnumerable<IValidateOptions<TOptions>> validations,
        IEnumerable<IConfigureTenantOptions<TOptions, TTenantInfo>> configureTenantOptions,
        ITenantContextAccessor<TTenantInfo> multiTenantContextAccessor)
        : base(configuration, configurationPrefix, configurationDefinitions, configureOptions, postConfigureOptions, validations )
    {
        _configureTenantOptions =
            configureTenantOptions as IConfigureTenantOptions<TOptions, TTenantInfo>[] ??
            new List<IConfigureTenantOptions<TOptions, TTenantInfo>>(configureTenantOptions).ToArray();
        _multiTenantContextAccessor = multiTenantContextAccessor;
    }

    protected override TOptions CreateInstance(string name)
    {
        if (!_multiTenantContextAccessor.CurrentTenant.Resolved(out var tenantInfo, out _))
        {
            throw new CannotResolveTenantConfigurationWhenTenantContextIsNotResolved(typeof(TOptions));
        }

        _tenantInfo = tenantInfo;
        var options = base.CreateInstance(name);
        foreach (var configure in _configureTenantOptions)
        {
            configure.Configure(options, tenantInfo);
        }
        return options;
    }

    protected override string GetConfigurationPath(ConfigurationObjectDefinition<TOptions> definition)
        => GetConfigurationPathWithPrefix(ConfigurationPath.Combine(_tenantInfo.Id, definition.ConfigurationPath));
}
