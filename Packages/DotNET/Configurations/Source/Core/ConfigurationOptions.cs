// Copyright (c) woksin-org. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Extensions.Configuration;

namespace Woksin.Extensions.Configurations;

/// <summary>
/// Represents the custom configuration options.
/// </summary>
public class ConfigurationOptions
{
    /// <summary>
    /// Gets or sets the <see cref="BinderOptions"/>.
    /// </summary>
    public BinderOptions BinderOptions { get; set; } = new();
    
    /// <summary>
    /// Gets or sets the value indicating whether to validate data annotations on the configuration object.
    /// </summary>
    /// <remarks>This is set to true by default.</remarks>
    public bool ValidateDataAnnotations { get; set; } = true;

    /// <summary>
    /// Gets or sets the value indicating whether to validate the configuration object on startup.
    /// </summary>
    /// <remarks>This is set to false by default.</remarks>
    public bool ValidateOnStartup { get; set; } = false;
}
