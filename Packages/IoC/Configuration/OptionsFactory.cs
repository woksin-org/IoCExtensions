// Copyright (c) woksin-org. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using IoCExtensions.Configuration.Parsing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace IoCExtensions.Configuration;

/// <summary>
/// Represents an implementation of <see cref="Microsoft.Extensions.Options.OptionsFactory{TOptions}"/> specific for IoCExtensions configuration.
/// </summary>
/// <typeparam name="TOptions">The <see cref="Type"/> of the IoCExtensions configuration.</typeparam>
[DisableAutoRegistration]
class OptionsFactory<TOptions> : Microsoft.Extensions.Options.OptionsFactory<TOptions>
    where TOptions : class
{
	readonly IConfiguration _configuration;
	readonly IEnumerable<ConfigurationObjectDefinition<TOptions>> _definitions;
	readonly IParseConfigurationObjects _parser;
    readonly string _configurationPrefix;

    /// <summary>
    /// Initializes a new instance of the <see cref="OptionsFactory{TOptions}"/> class.
    /// </summary>
    /// <param name="configuration">The <see cref="IConfiguration"/>.</param>
    /// <param name="definitions">The <see cref="IEnumerable{T}"/> of <see cref="ConfigurationObjectDefinition{TOptions}"/>.</param>
    /// <param name="parser">The <see cref="IParseConfigurationObjects"/>.</param>
    /// <param name="setups">The <see cref="IEnumerable{T}"/> of <see cref="IConfigureOptions{TOptions}"/>.</param>
    /// <param name="postConfigures">The <see cref="IEnumerable{T}"/> of <see cref="IPostConfigureOptions{TOptions}"/>.</param>
    /// <param name="validations">The <see cref="IEnumerable{T}"/> of <see cref="IValidateOptions{TOptions}"/>.</param>
    /// <param name="configurationOptionsConfigurators">The <see cref="IEnumerable{T}"/> of <see cref="IConfigureOptions{TOptions}"/> that configures "/><see cref="IoCExtensionsConfigurationOptions"/>.</param>
    public OptionsFactory(
	    IConfiguration configuration,
        IEnumerable<ConfigurationObjectDefinition<TOptions>> definitions,
        IParseConfigurationObjects parser,
        IEnumerable<IConfigureOptions<TOptions>> setups, 
        IEnumerable<IPostConfigureOptions<TOptions>> postConfigures,
        IEnumerable<IValidateOptions<TOptions>> validations,
        IEnumerable<IConfigureOptions<IoCExtensionsConfigurationOptions>> configurationOptionsConfigurators)
        : base(setups, postConfigures, validations)
    {
	    _configuration = configuration;
        _definitions = definitions;
        _parser = parser;
        var options = new IoCExtensionsConfigurationOptions();
        foreach (var configurator in configurationOptionsConfigurators)
        {
	        configurator?.Configure(options);
        }

        _configurationPrefix = options.Prefix;
    }

    /// <inheritdoc />
    protected override TOptions CreateInstance(string name)
    {
        var definition = _definitions.FirstOrDefault();
        if (definition == default)
        {
            return base.CreateInstance(name);
        }

        var sectionPath = GetConfigurationSection(definition);
        var section = _configuration.GetSection(sectionPath);
        var succeededParsing = _parser.TryParseFrom(section, out TOptions? instance, out var error);
        if (!succeededParsing)
        {
            throw new CannotParseConfiguration(error!, typeof(TOptions), sectionPath);
        }

        return instance!;
    }

    /// <summary>
    /// Gets the configuration section of an <see cref="IConfiguration"/> for a IoCExtensions configuration.
    /// </summary>
    /// <param name="definition">The <see cref="ConfigurationObjectDefinition{TOptions}"/> of the IoCExtensions configuration to get the configuration section for.</param>
    /// <returns>The configuration section string.</returns>
    // ReSharper disable once SuggestBaseTypeForParameter
    string GetConfigurationSection(ConfigurationObjectDefinition<TOptions> definition)
	    => GetConfigurationSectionWithPrefix(definition.Section);

    /// <summary>
    /// Gets the section string prefixed correctly.
    /// </summary>
    /// <param name="section">The configuration section to prefix.</param>
    /// <returns>The correctly prefixed configuration section string.</returns>
    string GetConfigurationSectionWithPrefix(string section)
        => $"{_configurationPrefix}{section}";
}
