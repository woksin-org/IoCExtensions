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
    /// <param name="binderOptions">The <see cref="BinderOptions"/>.</param>
    /// <param name="configurationPathFirstPart">The first configuration path part.</param>
    /// <param name="configurationPathRestParts">The configuration path parts to parse the object from, excluding the prefix that's configured.</param>
    protected BaseConfigurationAttribute(BinderOptions binderOptions, string configurationPathFirstPart, params string[] configurationPathRestParts)
    {
        ConfigurationPath = configurationPathFirstPart;
        if (configurationPathRestParts.Length > 0)
        {
            ConfigurationPath = Microsoft.Extensions.Configuration.ConfigurationPath.Combine(
                ConfigurationPath,
                Microsoft.Extensions.Configuration.ConfigurationPath.Combine(configurationPathRestParts));
        }
        BinderOptions = binderOptions ?? new BinderOptions();
    }

    /// <summary>
    /// Gets the configuration path to parse the configuration object from.
    /// </summary>
    public string ConfigurationPath { get; }

    /// <summary>
    /// Gets the <see cref="BinderOptions"/>.
    /// </summary>
    public BinderOptions BinderOptions { get; }
}
