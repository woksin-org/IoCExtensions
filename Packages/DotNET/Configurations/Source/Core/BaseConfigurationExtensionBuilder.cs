// Copyright (c) woksin-org. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Woksin.Extensions.Configurations.Internal;

public abstract class BaseConfigurationExtensionBuilder<TBuilder> where TBuilder : BaseConfigurationExtensionBuilder<TBuilder>
{
    protected BaseConfigurationExtensionBuilder(IServiceCollection services, string[] configurationPrefixes)
    {
        Services = services;
        AddConfigurationPrefix(configurationPrefixes);

        // Replace the standard generic IOptionsFactory
        services.Add(ServiceDescriptor.Transient(typeof(IOptionsFactory<>), typeof(ConfigurationsExtensionOptionsFactory<>)));
    }
    protected IServiceCollection Services { get; }

    protected abstract TBuilder Builder { get; }

    public TBuilder WithAssembly(Assembly assembly)
    {
        foreach (var type in assembly.GetTypes())
        {
            AddForType(type);
        }

        return Builder;
    }

    protected virtual void AddForType(Type type)
    {
        var attribute = type.GetCustomAttribute<ConfigurationAttribute>();
        if (attribute is not null)
        {
            AddConfiguration(type, attribute.BinderOptions, attribute.ConfigurationPath);
        }
    }

    /// <summary>
    /// Adds a configuration object definition.
    /// </summary>
    /// <param name="configurationPathParts">The configuration path parts of the configuration object.</param>
    /// <typeparam name="TConfigurationType">The <see cref="Type"/> of the <see cref="ConfigurationObjectDefinition{TConfiguration}"/>.</typeparam>
    public TBuilder AddConfiguration<TConfigurationType>(params string[] configurationPathParts)
        => AddConfiguration(typeof(TConfigurationType), configurationPathParts);

    /// <summary>
    /// Adds a configuration object definition.
    /// </summary>
    /// <param name="binderOptions">The <see cref="BinderOptions"/>.</param>
    /// <param name="configurationPathParts">The configuration path parts of the configuration object.</param>
    /// <typeparam name="TConfigurationType">The <see cref="Type"/> of the <see cref="ConfigurationObjectDefinition{TConfiguration}"/>.</typeparam>
    public TBuilder AddConfiguration<TConfigurationType>(BinderOptions binderOptions, params string[] configurationPathParts)
        => AddConfiguration(typeof(TConfigurationType), binderOptions, configurationPathParts);

    /// <summary>
    /// Adds a configuration object definition.
    /// </summary>
    /// <param name="type">The <see cref="Type"/> of the <see cref="ConfigurationObjectDefinition{TConfiguration}"/>.</param>
    /// <param name="configurationPathParts">The configuration path parts of the configuration object.</param>
    public TBuilder AddConfiguration(Type type, params string[] configurationPathParts)
        => AddConfigurationObjectDefinitionFor(type, ConfigurationPath.Combine(configurationPathParts), binderOptions: null);

    /// <summary>
    /// Adds a configuration object definition.
    /// </summary>
    /// <param name="type">The <see cref="Type"/> of the <see cref="ConfigurationObjectDefinition{TConfiguration}"/>.</param>
    /// <param name="binderOptions">The <see cref="BinderOptions"/>.</param>
    /// <param name="configurationPathParts">The configuration path parts of the configuration object.</param>
    public TBuilder AddConfiguration(Type type, BinderOptions binderOptions, params string[] configurationPathParts)
        => AddConfigurationObjectDefinitionFor(type, ConfigurationPath.Combine(configurationPathParts), binderOptions);

    /// <summary>
    /// Adds a configuration object definition.
    /// </summary>
    /// <param name="type">The <see cref="Type"/> of the <see cref="ConfigurationObjectDefinition{TConfiguration}"/>.</param>
    /// <param name="configurationPath">The configuration path of the configuration object.</param>
    /// <param name="binderOptions">The optional <see cref="BinderOptions"/>.</param>
    protected TBuilder AddConfigurationObjectDefinitionFor(Type type, string configurationPath, BinderOptions? binderOptions)
    {
        ConfigurationAdder.AddConfigurationDefinitionToServices(Services, type, configurationPath, binderOptions ?? new BinderOptions());
        AddAdditionalServicesFor(type);
        return Builder;
    }

    void AddConfigurationPrefix(string[] configurationPrefixes)
    {
        var prefix = ConfigurationPath.Combine(configurationPrefixes);
        if (!string.IsNullOrEmpty(prefix))
        {
            prefix += ConfigurationPath.KeyDelimiter;
        }

        Services.AddSingleton(new ConfigurationPrefix(prefix));
    }

    protected abstract void AddAdditionalServicesFor(Type optionsType);
}
