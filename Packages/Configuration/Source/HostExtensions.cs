// Copyright (c) woksin-org. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Reflection;
using Configuration.Extension.Parsing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Configuration.Extension;

/// <summary>
/// Represents extension methods for adding the configuration system to a host.
/// </summary>
public static class HostExtensions
{
	/// <summary>
	/// Use the configuration system.
	/// </summary>
	/// <param name="builder">The <see cref="IHostBuilder"/>.</param>
    /// <param name="startupAssembly">The startup assembly.</param>
	/// <param name="configurationPrefixes">The configuration prefixes.</param>
	/// <returns>The builder for continuation.</returns>
	public static IHostBuilder UseConfigurationExtension(this IHostBuilder builder, Assembly startupAssembly, params string[] configurationPrefixes)
        => builder.ConfigureServices((_, services) => services.AddConfigurationExtension(startupAssembly, configurationPrefixes));

    /// <summary>
    /// Adds the configuration system.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/>.</param>
    /// <param name="startupAssembly">The startup assembly.</param>
    /// <param name="configurationPrefixes">The configuration prefixes.</param>
    /// <returns>The builder for continuation.</returns>
    public static IServiceCollection AddConfigurationExtension(this IServiceCollection services, Assembly startupAssembly, params string[] configurationPrefixes)
	{
		ConfigureSettings(services, configurationPrefixes);
		services.Add(ServiceDescriptor.Singleton(typeof(IOptionsFactory<>), typeof(OptionsFactory<>)));
		services.AddSingleton<IParseConfigurationObjects, ConfigurationParser>();
        foreach (var type in startupAssembly.GetTypes())
        {
            var attribute = type.GetCustomAttribute<ConfigurationAttribute>();
            if (attribute is not null)
            {
                AddConfigurationObjectDefinitionFor(services, type, attribute.ConfigurationPath);
            }
        }

        return services;
	}
    
    /// <summary>
    /// Adds a single configuration object definition.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/>.</param>
    /// <param name="configurationPathParts">The configuration path parts of the configuration object.</param>
    /// <typeparam name="TConfigurationType">The <see cref="Type"/> of the <see cref="ConfigurationObjectDefinition{TConfiguration}"/>.</typeparam>
    public static void AddConfigurationObjectDefinitionFor<TConfigurationType>(this IServiceCollection services, params string[] configurationPathParts)
        => services.AddConfigurationObjectDefinitionFor(typeof(TConfigurationType), configurationPathParts);


    /// <summary>
    /// Adds a single configuration object definition.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/>.</param>
    /// <param name="type">The <see cref="Type"/> of the <see cref="ConfigurationObjectDefinition{TConfiguration}"/>.</param>
    /// <param name="configurationPathParts">The configuration path parts of the configuration object.</param>
    public static void AddConfigurationObjectDefinitionFor(this IServiceCollection services, Type type, params string[] configurationPathParts)
        => AddConfigurationObjectDefinition(services, type, ConfigurationPath.Combine(configurationPathParts));


    static void ConfigureSettings(IServiceCollection services, string[] configurationPrefixes)
    {
        var prefix = ConfigurationPath.Combine(configurationPrefixes);
        if (!string.IsNullOrEmpty(prefix))
        {
            prefix += ConfigurationPath.KeyDelimiter;
        }

        services.AddOptions<Settings>().Configure(_ => _.Prefix = prefix);
    }

    static void AddConfigurationObjectDefinition(IServiceCollection services, Type type, string configurationPath)
    {
        var definitionType = typeof(ConfigurationObjectDefinition<>).MakeGenericType(type);
        var definition = Activator.CreateInstance(definitionType, configurationPath)!;
        services.AddSingleton(definitionType, definition);
        services.AddSingleton(typeof(IAmAConfigurationObjectDefinition), _ => _.GetRequiredService(definitionType));
    }
}
