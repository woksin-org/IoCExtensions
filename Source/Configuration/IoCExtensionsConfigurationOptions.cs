namespace IoCExtensions.Configuration;

/// <summary>
/// Represents the configuration of the IoCExtensions configuration system.
/// </summary>
public class IoCExtensionsConfigurationOptions
{
	/// <summary>
	/// Gets or sets the configuration path prefix used by the IoCExtensions configuration system when reading in configuration
	/// objects when <see cref="ConfigurationAttribute"/> decorator.  
	/// </summary>
	public string Prefix { get; set; } = "";
}
