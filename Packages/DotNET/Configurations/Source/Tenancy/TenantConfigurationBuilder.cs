// Copyright (c) woksin-org. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Woksin.Extensions.Configurations.Internal;
using Woksin.Extensions.Tenancy;

namespace Woksin.Extensions.Configurations.Tenancy;

public class TenantConfigurationBuilder<TTenant> : BaseConfigurationBuilder<TenantConfigurationBuilder<TTenant>>
    where TTenant : class, ITenantInfo, new()
{
    protected override TenantConfigurationBuilder<TTenant> Builder => this;

    public TenantConfigurationBuilder(IServiceCollection services, string[] configurationPrefixes)
        : base(services, configurationPrefixes)
    {
    }

    public TenantConfigurationBuilder<TTenant> ConfigureTenancy(Action<TenancyBuilder<TTenant>>? configureTenancy = null, Action<TenancyOptions<TTenant>>? configureTenancyOptions = null, params string[] configurationPathParts)
    {
        var tenancyBuilder = Services.AddTenancyExtension<TTenant>();
        configureTenancy?.Invoke(tenancyBuilder);
        AddConfiguration<TenancyOptions<TTenant>>(configurationPathParts);
        configureTenancyOptions ??= _ => { };
        Services.Configure(configureTenancyOptions);
        return Builder;
    }

    /// <summary>
    /// Adds a configuration object definition.
    /// </summary>
    /// <param name="configurationPathParts">The configuration path parts of the configuration object.</param>
    /// <typeparam name="TConfigurationType">The <see cref="Type"/> of the <see cref="ConfigurationObjectDefinition{TConfiguration}"/>.</typeparam>
    public TenantConfigurationBuilder<TTenant> AddTenantConfiguration<TConfigurationType>(params string[] configurationPathParts)
        => AddTenantConfiguration(typeof(TConfigurationType), configurationPathParts);

    /// <summary>
    /// Adds a configuration object definition.
    /// </summary>
    /// <param name="binderOptions">The <see cref="Microsoft.Extensions.Configuration.BinderOptions"/>.</param>
    /// <param name="configurationPathParts">The configuration path parts of the configuration object.</param>
    /// <typeparam name="TConfigurationType">The <see cref="Type"/> of the <see cref="ConfigurationObjectDefinition{TConfiguration}"/>.</typeparam>
    public TenantConfigurationBuilder<TTenant> AddTenantConfiguration<TConfigurationType>(BinderOptions binderOptions, params string[] configurationPathParts)
        => AddTenantConfiguration(typeof(TConfigurationType), binderOptions, configurationPathParts);

    /// <summary>
    /// Adds a configuration object definition.
    /// </summary>
    /// <param name="type">The <see cref="Type"/> of the <see cref="ConfigurationObjectDefinition{TConfiguration}"/>.</param>
    /// <param name="configurationPathParts">The configuration path parts of the configuration object.</param>
    public TenantConfigurationBuilder<TTenant> AddTenantConfiguration(Type type, params string[] configurationPathParts)
        => AddTenantConfigurationObjectDefinitionFor(type, ConfigurationPath.Combine(configurationPathParts), binderOptions: null);

    /// <summary>
    /// Adds a configuration object definition.
    /// </summary>
    /// <param name="type">The <see cref="Type"/> of the <see cref="ConfigurationObjectDefinition{TConfiguration}"/>.</param>
    /// <param name="binderOptions">The <see cref="BinderOptions"/>.</param>
    /// <param name="configurationPathParts">The configuration path parts of the configuration object.</param>
    public TenantConfigurationBuilder<TTenant> AddTenantConfiguration(Type type, BinderOptions binderOptions, params string[] configurationPathParts)
        => AddTenantConfigurationObjectDefinitionFor(type, ConfigurationPath.Combine(configurationPathParts), binderOptions);

    TenantConfigurationBuilder<TTenant> AddTenantConfigurationObjectDefinitionFor(Type type, string configurationPath, BinderOptions? binderOptions)
    {
        ConfigurationAdder.AddConfigurationDefinitionToServices(Services, type, configurationPath, binderOptions ?? new BinderOptions());
        AddTenantSpecificServicesFor(type);
        return this;
    }

    /// <summary>
    /// Adds a <see cref="IConfigureTenantOptions{TOptions,TTenant}"/> that configures the specified <typeparamref name="TOption"/> configuration.
    /// </summary>
    /// <param name="configure">The callback to configure the options.</param>
    /// <typeparam name="TOption">The <see cref="Type"/> of the options to configure.</typeparam>
    /// <returns>The builder for continuation.</returns>
    public TenantConfigurationBuilder<TTenant> Configure<TOption>(Action<TOption, TTenant> configure)
        where TOption : class, new()
    {
        Services.AddSingleton<IConfigureTenantOptions<TOption, TTenant>>(new ConfigureTenantOptions<TOption, TTenant>(configure));
        return this;
    }

    protected override void AddForType(Type type, BaseConfigurationAttribute attribute)
    {
        if (attribute is TenantConfigurationAttribute tenantAttribute)
        {
            AddTenantConfiguration(type, tenantAttribute.BinderOptions, tenantAttribute.ConfigurationPath);
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
