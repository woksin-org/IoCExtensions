// Copyright (c) woksin-org. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Extensions.Configuration;

namespace Woksin.Extensions.Configurations;

/// <summary>
/// Represents the base attribute type for configuration objects.
/// </summary>
// ReSharper disable once ClassNeverInstantiated.Global
#pragma warning disable RCS1203
public abstract class BaseConfigurationAttribute : Attribute
#pragma warning restore RCS1203
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BaseConfigurationAttribute"/> class.
    /// </summary>
    /// <param name="configurationOptions">The <see cref="ConfigurationOptions"/>.</param>
    /// <param name="configurationPathParts">The configuration path parts to parse the object from, excluding the prefix that's configured.</param>
    protected BaseConfigurationAttribute(ConfigurationOptions configurationOptions, params string[] configurationPathParts)
    {
        if (configurationPathParts.Length > 0)
        {
            ConfigurationPath = Microsoft.Extensions.Configuration.ConfigurationPath.Combine(configurationPathParts);
        }
        ConfigurationOptions = configurationOptions ?? new ConfigurationOptions();
    }

    /// <summary>
    /// Gets the configuration path to parse the configuration object from.
    /// </summary>
    public string ConfigurationPath { get; } = "";

    /// <summary>
    /// Gets the <see cref="ConfigurationOptions"/>.
    /// </summary>
    public ConfigurationOptions ConfigurationOptions { get; }
}
