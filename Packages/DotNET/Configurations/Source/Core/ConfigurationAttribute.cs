// Copyright (c) woksin-org. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Extensions.Configuration;

namespace Woksin.Extensions.Configurations;

/// <summary>
/// Indicates that the type should be registered as a configuration object.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
// ReSharper disable once ClassNeverInstantiated.Global
public class ConfigurationAttribute : BaseConfigurationAttribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ConfigurationAttribute"/> class.
    /// </summary>
    /// <param name="configurationPathFirstPart">The first configuration path part.</param>
    /// <param name="configurationPathRestParts">The configuration path parts to parse the object from, excluding the prefix that's configured.</param>
    public ConfigurationAttribute(string configurationPathFirstPart, params string[] configurationPathRestParts)
        : this(new ConfigurationOptions(), configurationPathFirstPart, configurationPathRestParts)
    {
    }
    /// <summary>
    /// Initializes a new instance of the <see cref="ConfigurationAttribute"/> class.
    /// </summary>
    /// <param name="configurationOptions">The <see cref="ConfigurationOptions"/>.</param>
    /// <param name="configurationPathFirstPart">The first configuration path part.</param>
    /// <param name="configurationPathRestParts">The configuration path parts to parse the object from, excluding the prefix that's configured.</param>
    public ConfigurationAttribute(ConfigurationOptions configurationOptions, string configurationPathFirstPart, params string[] configurationPathRestParts)
        : base(configurationOptions, configurationPathFirstPart, configurationPathRestParts)
    {
    }
}
