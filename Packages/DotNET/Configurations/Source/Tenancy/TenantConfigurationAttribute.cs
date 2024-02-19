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
    public TenantConfigurationAttribute(string configurationPathFirstPart, params string[] configurationPathRestParts)
        : this(new ConfigurationOptions(), configurationPathFirstPart, configurationPathRestParts)
    {
    }

    public TenantConfigurationAttribute(ConfigurationOptions configurationOptions, string configurationPathFirstPart, params string[] configurationPathRestParts)
        : base(configurationOptions, configurationPathFirstPart, configurationPathRestParts)
    {
    }
}
