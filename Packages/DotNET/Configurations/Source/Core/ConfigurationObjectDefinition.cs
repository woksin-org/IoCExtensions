// Copyright (c) woksin-org. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Extensions.Configuration;

namespace Woksin.Extensions.Configurations;

/// <summary>
/// Represents the definition of a IoCExtensions configuration.
/// </summary>
/// <typeparam name="TConfiguration">The <see cref="Type"/> of the IoCExtensions configuration object.</typeparam>
public class ConfigurationObjectDefinition<TConfiguration> : IAmAConfigurationObjectDefinition
    where TConfiguration : class
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ConfigurationObjectDefinition{TOptions}"/> class.
    /// </summary>
    /// <param name="configurationPath">The configuration path.</param>
    /// <param name="configurationOptions">The configuration options.</param>
    public ConfigurationObjectDefinition(string configurationPath, ConfigurationOptions configurationOptions)
    {
	    ConfigurationPath = configurationPath;
        ConfigurationOptions = configurationOptions;
        ConfigurationObjectType = typeof(TConfiguration);
        if (string.IsNullOrWhiteSpace(ConfigurationPath))
        {
            ConfigurationPath = ConfigurationObjectType.Name;
        }
    }

    /// <inheritdoc />
    public string ConfigurationPath { get; }

    /// <inheritdoc />
    public ConfigurationOptions ConfigurationOptions { get; }

    /// <inheritdoc />
    public Type ConfigurationObjectType { get; }
}
