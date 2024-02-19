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
        : base(new ConfigurationOptions(), [configurationPathFirstPart, ..configurationPathRestParts])
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ConfigurationAttribute"/> class.
    /// </summary>
    /// <param name="validateOnStartup">Validates configuration at startup.</param>
    /// <param name="validateDataAnnotation"></param>
    /// <param name="bindNonPublicProperties"></param>
    /// <param name="errorOnUnknownConfiguration"></param>
    /// <param name="configurationPathParts">The configuration path parts to parse the object from, excluding the prefix that's configured.</param>
    public ConfigurationAttribute(
        bool validateOnStartup = false,
        bool validateDataAnnotation = true,
        bool bindNonPublicProperties = false,
        bool errorOnUnknownConfiguration = false,
        params string[] configurationPathParts)
        : base(new ConfigurationOptions
        {
            ValidateDataAnnotations = validateDataAnnotation,
            ValidateOnStartup = validateOnStartup,
            BinderOptions = new BinderOptions
            {
                BindNonPublicProperties = bindNonPublicProperties,
                ErrorOnUnknownConfiguration = errorOnUnknownConfiguration
            }
        }, configurationPathParts)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ConfigurationAttribute"/> class.
    /// </summary>
    /// <param name="configurationPathParts">The configuration path parts to parse the object from, excluding the prefix that's configured.</param>
    /// <param name="validateOnStartup">Validates configuration at startup.</param>
    /// <param name="validateDataAnnotation"></param>
    /// <param name="bindNonPublicProperties"></param>
    /// <param name="errorOnUnknownConfiguration"></param>
    public ConfigurationAttribute(
        string[] configurationPathParts,
        bool validateOnStartup = false,
        bool validateDataAnnotation = true,
        bool bindNonPublicProperties = false,
        bool errorOnUnknownConfiguration = false)
        : base(new ConfigurationOptions
        {
            ValidateDataAnnotations = validateDataAnnotation,
            ValidateOnStartup = validateOnStartup,
            BinderOptions = new BinderOptions
            {
                BindNonPublicProperties = bindNonPublicProperties,
                ErrorOnUnknownConfiguration = errorOnUnknownConfiguration
            }
        }, configurationPathParts)
    {
    }
}
