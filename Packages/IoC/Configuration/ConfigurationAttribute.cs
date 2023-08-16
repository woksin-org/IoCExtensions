using Microsoft.Extensions.Configuration;

namespace IoCExtensions.Configuration;

/// <summary>
/// Indicates that the type should be registered as a configuration object in a DI container.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
// ReSharper disable once ClassNeverInstantiated.Global
public class ConfigurationAttribute : Attribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ConfigurationAttribute"/> class.
    /// </summary>
    /// <param name="section">The configuration section to parse the object from, excluding the prefix that's configured with the <see cref="IoCExtensionsConfigurationOptions"/> configuration.</param>
    public ConfigurationAttribute(params string[] section)
    {
	    if (section.Length == 0) throw new ArgumentException("Configuration attribute must include one or more sections", nameof(section));
        Section = ConfigurationPath.Combine(section);
    }
    
    /// <summary>
    /// Gets the configuration section to parse the configuration object from.
    /// </summary>
    public string Section { get; }
}
