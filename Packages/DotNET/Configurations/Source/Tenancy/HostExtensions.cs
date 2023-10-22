// Copyright (c) woksin-org. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Woksin.Extensions.IoC;
using Woksin.Extensions.IoC.Tenancy;

namespace Woksin.Extensions.Configurations.Tenancy;

/// <summary>
/// Represents extension methods for adding the configuration system to a host.
/// </summary>
public static class HostExtensions
{
	/// <summary>
	/// Use the configuration system.
	/// </summary>
	/// <param name="builder">The <see cref="IHostBuilder"/>.</param>
	/// <param name="configurationPrefixes">The configuration prefixes.</param>
	/// <returns>The builder for continuation.</returns>
	public static IHostBuilder UseConfigurationExtension(this IHostBuilder builder, params string[] configurationPrefixes)
        => builder.ConfigureServices((_, services) => services.AddConfigurationExtension(configurationPrefixes));

    /// <summary>
    /// Adds the configuration system.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/>.</param>
    /// <param name="configurationPrefixes">The configuration prefixes.</param>
    /// <returns>The builder for continuation.</returns>
    public static IServiceCollection AddConfigurationExtension(this IServiceCollection services, params string[] configurationPrefixes)
    {
        services.Configure<IoCSettings>(settings => settings.AdditionalAssemblies.Add(typeof(HostExtensions).Assembly));
		AddConfigurationPrefix(services, configurationPrefixes);
		services.Add(ServiceDescriptor.Singleton(typeof(IOptionsFactory<>), typeof(ConfigurationsExtensionOptionsFactory<>)));
        services.AddTenantScopedServices(tenantServices => tenantServices.Add(ServiceDescriptor.Singleton(typeof(IOptionsFactory<>), typeof(ConfigurationsExtensionOptionsFactory<>))));
        return services;
	}

    /// <summary>
    /// Adds a configuration object definition.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/>.</param>
    /// <param name="configurationPathParts">The configuration path parts of the configuration object.</param>
    /// <typeparam name="TConfigurationType">The <see cref="Type"/> of the <see cref="ConfigurationObjectDefinition{TConfiguration}"/>.</typeparam>
    public static void AddConfigurationObjectDefinitionFor<TConfigurationType>(this IServiceCollection services, params string[] configurationPathParts)
        => services.AddConfigurationObjectDefinitionFor(typeof(TConfigurationType), configurationPathParts);

    /// <summary>
    /// Adds a configuration object definition.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/>.</param>
    /// <param name="type">The <see cref="Type"/> of the <see cref="ConfigurationObjectDefinition{TConfiguration}"/>.</param>
    /// <param name="configurationPathParts">The configuration path parts of the configuration object.</param>
    public static void AddConfigurationObjectDefinitionFor(this IServiceCollection services, Type type, params string[] configurationPathParts)
        => services.AddConfigurationObjectDefinitionFor(type, ConfigurationPath.Combine(configurationPathParts));

    /// <summary>
    /// Adds a configuration object definition.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/>.</param>
    /// <param name="type">The <see cref="Type"/> of the <see cref="ConfigurationObjectDefinition{TConfiguration}"/>.</param>
    /// <param name="configurationPath">The configuration path of the configuration object.</param>
    public static void AddConfigurationObjectDefinitionFor(this IServiceCollection services, Type type, string configurationPath)
        => AddConfigurationObjectDefinition(services, type, configurationPath, false);
    
    /// <summary>
    /// Adds a tenant configuration object definition.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/>.</param>
    /// <param name="configurationPathParts">The configuration path parts of the configuration object.</param>
    /// <typeparam name="TConfigurationType">The <see cref="Type"/> of the <see cref="ConfigurationObjectDefinition{TConfiguration}"/>.</typeparam>
    public static void AddTenantConfigurationObjectDefinitionFor<TConfigurationType>(this IServiceCollection services, params string[] configurationPathParts)
        => services.AddTenantConfigurationObjectDefinitionFor(typeof(TConfigurationType), configurationPathParts);


    /// <summary>
    /// Adds a tenant configuration object definition.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/>.</param>
    /// <param name="type">The <see cref="Type"/> of the <see cref="ConfigurationObjectDefinition{TConfiguration}"/>.</param>
    /// <param name="configurationPathParts">The configuration path parts of the configuration object.</param>
    public static void AddTenantConfigurationObjectDefinitionFor(this IServiceCollection services, Type type, params string[] configurationPathParts)
        => services.AddConfigurationObjectDefinitionFor(type, ConfigurationPath.Combine(configurationPathParts));

    /// <summary>
    /// Adds a tenant configuration object definition.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/>.</param>
    /// <param name="type">The <see cref="Type"/> of the <see cref="ConfigurationObjectDefinition{TConfiguration}"/>.</param>
    /// <param name="configurationPath">The configuration path of the configuration object.</param>
    public static void AddTenantConfigurationObjectDefinitionFor(this IServiceCollection services, Type type, string configurationPath)
        => AddConfigurationObjectDefinition(services, type, configurationPath, true);

    static void AddConfigurationPrefix(IServiceCollection serviceCollection, string[] configurationPrefixes)
    {
        var prefix = ConfigurationPath.Combine(configurationPrefixes);
        if (!string.IsNullOrEmpty(prefix))
        {
            prefix += ConfigurationPath.KeyDelimiter;
        }

        serviceCollection.TryAddSingleton(new ConfigurationPrefix(prefix));
    }

    static void AddConfigurationObjectDefinition(IServiceCollection services, Type type, string configurationPath, bool perTenant)
    {
        if (perTenant)
        {
            services.AddTenantScopedServices((tenantId, tenantServices) => AddConfigurationObjectDefinitionTypes(
                tenantServices,
                typeof(ConfigurationObjectDefinition<>).MakeGenericType(type),
                ConfigurationPath.Combine(tenantId, configurationPath)));
        }
        else
        {
            AddConfigurationObjectDefinitionTypes(services, typeof(ConfigurationObjectDefinition<>).MakeGenericType(type), configurationPath);
        }
    }

    static void AddConfigurationObjectDefinitionTypes(IServiceCollection services, Type definitionType, string configurationPath)
    {
        var definition = Activator.CreateInstance(definitionType, configurationPath)!;
        services.AddSingleton(definitionType, definition);
        services.AddSingleton(typeof(IAmAConfigurationObjectDefinition), provider => provider.GetRequiredService(definitionType));
    }
}
