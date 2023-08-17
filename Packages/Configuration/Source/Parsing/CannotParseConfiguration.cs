// Copyright (c) woksin-org. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Configuration.Extension.Parsing;

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
    { }
}
