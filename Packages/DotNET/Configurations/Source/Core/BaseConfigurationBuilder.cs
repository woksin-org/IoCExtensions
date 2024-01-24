// Copyright (c) woksin-org. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Woksin.Extensions.Configurations.Internal;

/// <summary>
/// Exception that gets thrown when a configuration type has more than 1 configuration decorator.
/// </summary>
public class NoMoreThanOneConfigurationAttributeAllowed : Exception
{
    public NoMoreThanOneConfigurationAttributeAllowed(Type optionsType, IEnumerable<Attribute> attributes)
        : base($"Configuration '{optionsType}' is decorated with multiple configuration attributes. [{string.Join(", ",  attributes)}]")
    {
    }
}

/// <summary>
/// Represents the base builder for the configuration system.
/// </summary>
/// <typeparam name="TBuilder"></typeparam>
public abstract class BaseConfigurationBuilder<TBuilder> where TBuilder : BaseConfigurationBuilder<TBuilder>
{
    protected BaseConfigurationBuilder(IServiceCollection services, string[] configurationPrefixes)
    {
        Services = services;
        AddConfigurationPrefix(configurationPrefixes);

        // Replace the standard generic IOptionsFactory
        services.Add(ServiceDescriptor.Transient(typeof(IOptionsFactory<>), typeof(ConfigurationsExtensionOptionsFactory<>)));
    }

    /// <summary>
    /// Gets the <see cref="IServiceCollection"/>.
    /// </summary>
    protected IServiceCollection Services { get; }

    /// <summary>
    /// Gets the specific builder used for chaining calls.
    /// </summary>
    protected abstract TBuilder Builder { get; }

    /// <summary>
    /// Adds an assembly for discovering configuration types.
    /// </summary>
    /// <param name="assembly">The <see cref="Assembly"/> to get types from.</param>
    /// <returns>The builder for continuation.</returns>
    /// <exception cref="NoMoreThanOneConfigurationAttributeAllowed">When options type has more than one configuration attribute.</exception>
    public TBuilder WithAssembly(Assembly assembly)
    {
        foreach (var type in assembly.GetTypes())
        {
            var configurationAttributes = type.GetCustomAttributes<BaseConfigurationAttribute>().ToArray();
            switch  (configurationAttributes.Length)
            {
                case <= 0:
                    continue;
                case > 1:
                    throw new NoMoreThanOneConfigurationAttributeAllowed(type, configurationAttributes);
                default:
                    AddForType(type, configurationAttributes[0]);
                    break;
            }
        }

        return Builder;
    }

    /// <summary>
    /// Overridable method that looks for the correct custom attribute and adds configuration service bindings for a configuration type.
    /// </summary>
    /// <param name="type">The type to check.</param>
    /// <param name="attribute">The <see cref="BaseConfigurationAttribute"/>.</param>
    protected virtual void AddForType(Type type, BaseConfigurationAttribute attribute)
    {
        if (attribute is ConfigurationAttribute configurationAttribute)
        {
            AddConfiguration(type, configurationAttribute.BinderOptions, configurationAttribute.ConfigurationPath);
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

    TBuilder AddConfigurationObjectDefinitionFor(Type type, string configurationPath, BinderOptions? binderOptions)
    {
        ConfigurationAdder.AddConfigurationDefinitionToServices(Services, type, configurationPath, binderOptions ?? new BinderOptions());
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
}
