namespace Woksin.Extensions.Configurations;

/// <summary>
/// Represents the configuration prefix.
/// </summary>
public class ConfigurationPrefix
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ConfigurationPrefix"/> class.
    /// </summary>
    /// <param name="value"></param>
    public ConfigurationPrefix(string value)
    {
        Value = value;
    }

    /// <summary>
    /// Gets the configuration prefix.
    /// </summary>
    public string Value { get; }
}
