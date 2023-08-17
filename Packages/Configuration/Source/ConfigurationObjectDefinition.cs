// Copyright (c) woksin-org. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Configuration.Extension;

/// <summary>
/// Represents the definition of a IoCExtensions configuration.
/// </summary>
/// <typeparam name="TConfiguration">The <see cref="Type"/> of the IoCExtensions configuration object.</typeparam>
class ConfigurationObjectDefinition<TConfiguration> : IAmAConfigurationObjectDefinition
    where TConfiguration : class
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ConfigurationObjectDefinition{TOptions}"/> class.
    /// </summary>
    /// <param name="configurationPath">The configuration path.</param>
    public ConfigurationObjectDefinition(string configurationPath)
    {
	    ConfigurationPath = configurationPath;
	    ConfigurationObjectType = typeof(TConfiguration);
    }
    
    /// <inheritdoc />
    public string ConfigurationPath { get; }

    /// <inheritdoc />
    public Type ConfigurationObjectType { get; }
}
