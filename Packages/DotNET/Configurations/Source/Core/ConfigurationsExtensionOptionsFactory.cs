// Copyright (c) woksin-org. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Woksin.Extensions.Configurations;

/// <summary>
/// Represents an implementation of <see cref="Microsoft.Extensions.Options.OptionsFactory{TOptions}"/> specific for the configuration system.
/// </summary>
/// <typeparam name="TOptions">The <see cref="Type"/> of the IoCExtensions configuration.</typeparam>
public class ConfigurationsExtensionOptionsFactory<TOptions> : OptionsFactory<TOptions>
    where TOptions : class
{
    readonly IConfiguration _configuration;
	readonly IEnumerable<ConfigurationObjectDefinition<TOptions>> _definitions;
    readonly string _configurationPrefix;

    /// <summary>
    /// Initializes a new instance of the <see cref="ConfigurationsExtensionOptionsFactory{TOptions}"/> class.
    /// </summary>
    /// <param name="configuration">The <see cref="IConfiguration"/>.</param>
    /// <param name="configurationPrefix">The configuration prefix.</param>
    /// <param name="definitions">The <see cref="IEnumerable{T}"/> of <see cref="ConfigurationObjectDefinition{TOptions}"/>.</param>
    /// <param name="setups">The <see cref="IEnumerable{T}"/> of <see cref="IConfigureOptions{TOptions}"/>.</param>
    /// <param name="postConfigures">The <see cref="IEnumerable{T}"/> of <see cref="IPostConfigureOptions{TOptions}"/>.</param>
    /// <param name="validations">The <see cref="IEnumerable{T}"/> of <see cref="IValidateOptions{TOptions}"/>.</param>
    public ConfigurationsExtensionOptionsFactory(
	    IConfiguration configuration,
        ConfigurationPrefix configurationPrefix,
        IEnumerable<ConfigurationObjectDefinition<TOptions>> definitions,
        IEnumerable<IConfigureOptions<TOptions>> setups,
        IEnumerable<IPostConfigureOptions<TOptions>> postConfigures,
        IEnumerable<IValidateOptions<TOptions>> validations)
        : base(setups, postConfigures, validations)
    {
	    _configuration = configuration;
        _definitions = definitions;
        _configurationPrefix = configurationPrefix.Value;
    }

    /// <inheritdoc />
    protected override TOptions CreateInstance(string name)
    {
        var definition = _definitions.FirstOrDefault();
        return definition == default
            ? base.CreateInstance(name)
            : BindConfiguration(GetConfigurationPath(definition), definition.BinderOptions);
    }

    protected TOptions BindConfiguration(string configurationPath, BinderOptions binderOptions)
    {
        var configurationSection = _configuration.GetSection(configurationPath);
        var instance = Activator.CreateInstance<TOptions>();
        configurationSection.Bind(instance, options =>
        {
            options.BindNonPublicProperties = binderOptions.BindNonPublicProperties;
            options.ErrorOnUnknownConfiguration = binderOptions.ErrorOnUnknownConfiguration;
        });
        return instance!;
    }

    /// <summary>
    /// Gets the configuration path of an <see cref="IConfiguration"/> for a IoCExtensions configuration.
    /// </summary>
    /// <param name="definition">The <see cref="ConfigurationObjectDefinition{TOptions}"/> of the IoCExtensions configuration to get the configuration path for.</param>
    /// <returns>The configuration path string.</returns>
    // ReSharper disable once SuggestBaseTypeForParameter
    protected virtual string GetConfigurationPath(ConfigurationObjectDefinition<TOptions> definition)
	    => GetConfigurationPathWithPrefix(definition.ConfigurationPath);

    /// <summary>
    /// Gets the configuration path string prefixed correctly.
    /// </summary>
    /// <param name="configurationPath">The configuration path to prefix.</param>
    /// <returns>The correctly prefixed configuration path string.</returns>
    protected string GetConfigurationPathWithPrefix(string configurationPath)
        => $"{_configurationPrefix}{configurationPath}";
}
