// Copyright (c) woksin-org. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Woksin.Extensions.Configurations;

/// <summary>
/// Indicates that the type should be registered as a configuration object.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
// ReSharper disable once ClassNeverInstantiated.Global
public sealed class ConfigurationAttribute : Attribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ConfigurationAttribute"/> class.
    /// </summary>
    /// <param name="configurationPathParts">The configuration path parts to parse the object from, excluding the prefix that's configured.</param>
    public ConfigurationAttribute(params string[] configurationPathParts)
    {
	    if (configurationPathParts.Length == 0)
        {
            throw new ArgumentException("Configuration attribute must include one or more sections", nameof(configurationPathParts));
        }

        ConfigurationPath = Microsoft.Extensions.Configuration.ConfigurationPath.Combine(configurationPathParts);
    }

    /// <summary>
    /// Gets the configuration path to parse the configuration object from.
    /// </summary>
    public string ConfigurationPath { get; }
}
