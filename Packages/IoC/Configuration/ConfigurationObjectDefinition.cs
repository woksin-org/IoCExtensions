// Copyright (c) woksin-org. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace IoCExtensions.Configuration;

/// <summary>
/// Represents the definition of a IoCExtensions configuration.
/// </summary>
/// <typeparam name="TConfiguration">The <see cref="System.Type"/> of the IoCExtensions configuration object.</typeparam>
class ConfigurationObjectDefinition<TConfiguration> : IAmAConfigurationObjectDefinition
    where TConfiguration : class
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ConfigurationObjectDefinition{TOptions}"/> class.
    /// </summary>
    /// <param name="attribute">The <see cref="ConfigurationAttribute"/>.</param>
    public ConfigurationObjectDefinition(ConfigurationAttribute attribute)
    {
	    Section = attribute.Section;
	    ConfigurationObjectType = typeof(TConfiguration);
    }
    
    /// <inheritdoc />
    public string Section { get; }

    /// <inheritdoc />
    public Type ConfigurationObjectType { get; }
}
