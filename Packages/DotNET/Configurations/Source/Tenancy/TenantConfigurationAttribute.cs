// Copyright (c) woksin-org. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Extensions.Configuration;

namespace Woksin.Extensions.Configurations.Tenancy;

/// <summary>
/// Indicates that the type should be registered as a tenant configuration object.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
public class TenantConfigurationAttribute : ConfigurationAttribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TenantConfigurationAttribute"/> class.
    /// </summary>
    /// <param name="configurationPathFirstPart">The first configuration path part.</param>
    /// <param name="configurationPathRestParts">The configuration path parts to parse the object from, excluding the prefix that's configured.</param>
    public TenantConfigurationAttribute(string configurationPathFirstPart, params string[] configurationPathRestParts)
        : base(configurationPathFirstPart, configurationPathRestParts)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TenantConfigurationAttribute"/> class.
    /// </summary>
    /// <param name="validateOnStartup"></param>
    /// <param name="validateDataAnnotation"></param>
    /// <param name="bindNonPublicProperties"></param>
    /// <param name="errorOnUnknownConfiguration"></param>
    /// <param name="configurationPathParts">The configuration path parts to parse the object from, excluding the prefix that's configured.</param>
    public TenantConfigurationAttribute(
        bool validateOnStartup = false,
        bool validateDataAnnotation = true,
        bool bindNonPublicProperties = false,
        bool errorOnUnknownConfiguration = false,
        params string[] configurationPathParts)
        : base(
            validateOnStartup: validateOnStartup,
            validateDataAnnotation: validateDataAnnotation,
            bindNonPublicProperties:bindNonPublicProperties,
            errorOnUnknownConfiguration:errorOnUnknownConfiguration,
            configurationPathParts)
    {
    }
    /// <summary>
    /// Initializes a new instance of the <see cref="TenantConfigurationAttribute"/> class.
    /// </summary>
    /// <param name="configurationPathParts">The configuration path parts to parse the object from, excluding the prefix that's configured.</param>
    /// <param name="validateOnStartup"></param>
    /// <param name="validateDataAnnotation"></param>
    /// <param name="bindNonPublicProperties"></param>
    /// <param name="errorOnUnknownConfiguration"></param>
    public TenantConfigurationAttribute(
        string[] configurationPathParts,
        bool validateOnStartup = false,
        bool validateDataAnnotation = true,
        bool bindNonPublicProperties = false,
        bool errorOnUnknownConfiguration = false)
        : base(
            configurationPathParts,
            validateOnStartup: validateOnStartup,
            validateDataAnnotation: validateDataAnnotation,
            bindNonPublicProperties:bindNonPublicProperties,
            errorOnUnknownConfiguration:errorOnUnknownConfiguration)
    {
    }
}
