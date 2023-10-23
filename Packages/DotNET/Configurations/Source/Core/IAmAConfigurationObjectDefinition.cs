// Copyright (c) woksin-org. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Extensions.Configuration;

namespace Woksin.Extensions.Configurations;

/// <summary>
/// Defines a configuration object definition.
/// </summary>
public interface IAmAConfigurationObjectDefinition
{
    /// <summary>
    /// Gets the section where this configuration resides in the <see cref="Microsoft.Extensions.Configuration.IConfiguration"/>.
    /// </summary>
    public string ConfigurationPath { get; }

    /// <summary>
    /// Gets the <see cref="Type"/> of the configuration object.
    /// </summary>
    public Type ConfigurationObjectType { get; }
    
    /// <summary>
    /// Gets the <see cref="BinderOptions"/>.
    /// </summary>
    public BinderOptions BinderOptions { get; }
}
