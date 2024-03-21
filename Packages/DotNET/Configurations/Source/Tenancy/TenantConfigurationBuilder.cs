// Copyright (c) woksin-org. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Woksin.Extensions.Configurations.Internal;
using Woksin.Extensions.Tenancy;

namespace Woksin.Extensions.Configurations.Tenancy;

/// <summary>
/// Represents a builder that builds the tenant configuration system and the underlying tenancy system.
/// </summary>
/// <typeparam name="TTenant">The <see cref="Type"/> of the <see cref="ITenantInfo"/>.</typeparam>
/// <remarks>ConfigureTenancy has to be called at some point.</remarks>
public class TenantConfigurationBuilder<TTenant> : BaseConfigurationBuilder<TenantConfigurationBuilder<TTenant>>
    where TTenant : class, ITenantInfo, new()
{
    protected override TenantConfigurationBuilder<TTenant> Builder => this;

    readonly TenancyBuilder<TTenant> _tenancyBuilder;
    bool _tenancyConfigAdded;

    public TenantConfigurationBuilder(IServiceCollection services, string[] configurationPrefixes)
        : base(services, configurationPrefixes)
    {
        _tenancyBuilder = Services.AddTenancyExtension<TTenant>();
        Services.TryAddSingleton<ITenantOptions<TTenant>, TenantOptions<TTenant>>();
        Services.TryAddSingleton<ITenantOptions>(sp => (ITenantOptions)sp.GetRequiredService<ITenantOptions<TTenant>>());
    }

    /// <summary>
    /// Configures the underlying tenancy system.
    /// </summary>
    /// <param name="configureTenancy">Callback for configuring tenancy using <see cref="TenancyBuilder{TTenant}"/>.</param>
    /// <param name="configureTenancyOptions">Callback for configuring <see cref="TenancyOptions{TTenant}"/>.</param>
    /// <param name="tenancyConfigurationPathParts">The optional configuration path parts to the <see cref="TenancyOptions{TTenant}"/> configuration. If no path is given then it defaults to "Tenancy".</param>
    /// <returns>The builder for continuation.</returns>
    public TenantConfigurationBuilder<TTenant> ConfigureTenancy(Action<TenancyBuilder<TTenant>>? configureTenancy = null, Action<TenancyOptions<TTenant>>? configureTenancyOptions = null, params string[] tenancyConfigurationPathParts)
    {
        if (!_tenancyConfigAdded)
        {
            AddTenancyConfiguration(tenancyConfigurationPathParts);
            _tenancyConfigAdded = true;
        }
        configureTenancy?.Invoke(_tenancyBuilder);
        if (configureTenancyOptions is not null)
        {
            Services.Configure(configureTenancyOptions);
        }
        return Builder;
    }

    void AddTenancyConfiguration(string[] tenancyConfigurationPathParts)
    {
        AddConfiguration<TenancyOptions<TTenant>>(tenancyConfigurationPathParts.Length == 0 ? ["Tenancy"] : tenancyConfigurationPathParts);
    }
    
    /// <summary>
    /// Adds a tenant configuration object definition.
    /// </summary>
    /// <param name="configurationPathParts">The configuration path parts of the configuration object.</param>
    /// <typeparam name="TConfigurationType">The <see cref="Type"/> of the <see cref="ConfigurationObjectDefinition{TConfiguration}"/>.</typeparam>
    public TenantConfigurationBuilder<TTenant> AddTenantConfiguration<TConfigurationType>(params string[] configurationPathParts)
        => AddTenantConfiguration(typeof(TConfigurationType), configurationPathParts);

    /// <summary>
    /// Adds a tenant configuration object definition.
    /// </summary>
    /// <param name="configurationOptions">The <see cref="ConfigurationOptions"/>.</param>
    /// <param name="configurationPathParts">The configuration path parts of the configuration object.</param>
    /// <typeparam name="TConfigurationType">The <see cref="Type"/> of the <see cref="ConfigurationObjectDefinition{TConfiguration}"/>.</typeparam>
    public TenantConfigurationBuilder<TTenant> AddTenantConfiguration<TConfigurationType>(ConfigurationOptions configurationOptions, params string[] configurationPathParts)
        => AddTenantConfiguration(typeof(TConfigurationType), configurationOptions, configurationPathParts);

    /// <summary>
    /// Adds a tenant configuration object definition.
    /// </summary>
    /// <param name="type">The <see cref="Type"/> of the <see cref="ConfigurationObjectDefinition{TConfiguration}"/>.</param>
    /// <param name="configurationPathParts">The configuration path parts of the configuration object.</param>
    public TenantConfigurationBuilder<TTenant> AddTenantConfiguration(Type type, params string[] configurationPathParts)
        => AddTenantConfigurationObjectDefinitionFor(type, ConfigurationPath.Combine(configurationPathParts), configurationOptions: null);

    /// <summary>
    /// Adds a tenant configuration object definition.
    /// </summary>
    /// <param name="type">The <see cref="Type"/> of the <see cref="ConfigurationObjectDefinition{TConfiguration}"/>.</param>
    /// <param name="configurationOptions">The <see cref="ConfigurationOptions"/>.</param>
    /// <param name="configurationPathParts">The configuration path parts of the configuration object.</param>
    public TenantConfigurationBuilder<TTenant> AddTenantConfiguration(Type type, ConfigurationOptions configurationOptions, params string[] configurationPathParts)
        => AddTenantConfigurationObjectDefinitionFor(type, ConfigurationPath.Combine(configurationPathParts), configurationOptions);

    TenantConfigurationBuilder<TTenant> AddTenantConfigurationObjectDefinitionFor(Type type, string configurationPath, ConfigurationOptions? configurationOptions)
    {
        ConfigurationAdder.AddConfigurationDefinitionToServices(Services, type, configurationPath, configurationOptions ?? new ConfigurationOptions());
        AddTenantSpecificServicesFor(type);
        return this;
    }

    /// <summary>
    /// Adds a <see cref="IConfigureTenantOptions{TOptions,TTenant}"/> that configures the specified tenant <typeparamref name="TOption"/> configuration.
    /// </summary>
    /// <param name="configure">The callback to configure the options.</param>
    /// <typeparam name="TOption">The <see cref="Type"/> of the options to configure.</typeparam>
    /// <returns>The builder for continuation.</returns>
    public TenantConfigurationBuilder<TTenant> Configure<TOption>(Action<TOption, TTenant> configure)
        where TOption : class
    {
        Services.AddSingleton<IConfigureTenantOptions<TOption, TTenant>>(new ConfigureTenantOptions<TOption, TTenant>(configure));
        return this;
    }

    protected override void AddForType(Type type, BaseConfigurationAttribute attribute)
    {
        if (attribute is TenantConfigurationAttribute tenantAttribute)
        {
            AddTenantConfiguration(type, tenantAttribute.ConfigurationOptions, tenantAttribute.ConfigurationPath);
            AddTenantSpecificServicesFor(type);
        }
        else
        {
            base.AddForType(type, attribute);
        }
    }

    void AddTenantSpecificServicesFor(Type optionsType)
    {
        Services.TryAdd(CreateServiceDescriptor(optionsType, typeof(IOptionsMonitorCache<>), typeof(TenantOptionsCache<,>), ServiceLifetime.Singleton));
        Services.TryAdd(CreateServiceDescriptor(optionsType, typeof(IOptionsFactory<>), typeof(TenantOptionsFactory<,>), ServiceLifetime.Transient));
        Services.TryAdd(CreateOptionsManagerServiceDescriptor(optionsType, typeof(IOptionsSnapshot<>), ServiceLifetime.Scoped));
        Services.TryAdd(CreateOptionsManagerServiceDescriptor(optionsType, typeof(IOptions<>), ServiceLifetime.Singleton));
    }

    static ServiceDescriptor CreateOptionsManagerServiceDescriptor(Type optionsType, Type genericServiceType, ServiceLifetime serviceLifetime)
    {
        var serviceType = genericServiceType.MakeGenericType(optionsType);
        return new ServiceDescriptor(serviceType, sp => BuildOptionsManager(sp, optionsType), serviceLifetime);
    }

    static object BuildOptionsManager(IServiceProvider sp, Type optionsType)
    {
        var cache = ActivatorUtilities.CreateInstance(sp,
            typeof(TenantOptionsCache<,>).MakeGenericType(optionsType, typeof(TTenant)));
        return
            ActivatorUtilities.CreateInstance(sp, typeof(TenantOptionsManager<>).MakeGenericType(optionsType), cache);
    }

    static ServiceDescriptor CreateServiceDescriptor(Type optionsType, Type genericServiceType, Type genericImplementationType, ServiceLifetime serviceLifetime)
    {
        var serviceType = genericServiceType.MakeGenericType(optionsType);
        var implementationType = genericImplementationType.MakeGenericType(optionsType, typeof(TTenant));
        return new ServiceDescriptor(serviceType, implementationType, serviceLifetime);
    }
}
