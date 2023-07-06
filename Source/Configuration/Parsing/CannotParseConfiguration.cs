namespace IoCExtensions.Configuration.Parsing;

/// <summary>
/// Exception that gets thrown when a configuration type cannot be parsed.
/// </summary>
public class CannotParseConfiguration : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CannotParseConfiguration"/> class.
    /// </summary>
    public CannotParseConfiguration(Exception error, Type configuration, string section)
        : base($"Cannot parse {configuration} from section {section}", error)
    {
    }
}
