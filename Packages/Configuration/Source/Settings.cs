// Copyright (c) woksin-org. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Configuration.Extension;

/// <summary>
/// Represents the settings of the configuration system.
/// </summary>
public class Settings
{
	/// <summary>
	/// Gets or sets the configuration path prefix used by the configuration system when reading in configuration
	/// objects when <see cref="ConfigurationAttribute"/> decorator.  
	/// </summary>
	public string Prefix { get; set; } = "";
}
