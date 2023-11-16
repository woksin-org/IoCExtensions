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
    /// <param name="binderOptions">The binder options.</param>
    public ConfigurationObjectDefinition(string configurationPath, BinderOptions binderOptions)
    {
	    ConfigurationPath = configurationPath;
        BinderOptions = binderOptions;
        ConfigurationObjectType = typeof(TConfiguration);
    }

    /// <inheritdoc />
    public string ConfigurationPath { get; }

    /// <inheritdoc />
    public BinderOptions BinderOptions { get; }

    /// <inheritdoc />
    public Type ConfigurationObjectType { get; }
}
