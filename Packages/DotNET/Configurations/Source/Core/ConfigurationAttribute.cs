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
        : this(new BinderOptions(), configurationPathFirstPart, configurationPathRestParts)
    {
    }
    /// <summary>
    /// Initializes a new instance of the <see cref="ConfigurationAttribute"/> class.
    /// </summary>
    /// <param name="binderOptions">The <see cref="BinderOptions"/>.</param>
    /// <param name="configurationPathFirstPart">The first configuration path part.</param>
    /// <param name="configurationPathRestParts">The configuration path parts to parse the object from, excluding the prefix that's configured.</param>
    public ConfigurationAttribute(BinderOptions binderOptions, string configurationPathFirstPart, params string[] configurationPathRestParts)
        : base(binderOptions, configurationPathFirstPart, configurationPathRestParts)
    {
    }
}
