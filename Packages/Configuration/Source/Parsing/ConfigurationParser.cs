// Copyright (c) woksin-org. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Configuration.Extension.Parsing;

/// <summary>
/// Represents an implementation of <see cref="IParseConfigurationObjects"/>.
/// </summary>
class ConfigurationParser : IParseConfigurationObjects
{
	static readonly JsonSerializer _jsonSerializer = JsonSerializer.Create();

    /// <inheritdoc />
    public bool TryParseFrom<TOptions>(IConfigurationSection configuration, [NotNullWhen(true)]out TOptions? parsed, [NotNullWhen(false)]out Exception? error)
        where TOptions : class
    {
	    parsed = default;
	    error = default;
	    try
	    {
		    if (!TryReadConfiguration(
			        _ => WriteSectionToStream(configuration, _),
			        out var reader,
			        out error))
		    {
			    return false;
		    }
		    parsed = _jsonSerializer.Deserialize<TOptions>(reader)!;
		    return true;
	    }
	    catch (Exception ex)
	    {
		    error = ex;
		    return false;
	    }
    }
    
    static bool TryReadConfiguration(Action<TextWriter> writeToStream, [NotNullWhen(true)]out JsonTextReader? reader, [NotNullWhen(false)]out Exception? error)
    {
	    reader = default;
	    error = default;
	    try
	    {
		    var stream = new MemoryStream();
		    var writer = new StreamWriter(stream);
		    writeToStream(writer);
		    writer.Flush();

		    stream.Seek(0, SeekOrigin.Begin);
		    reader = new JsonTextReader(new StreamReader(stream));
		    return true;
	    }
	    catch (Exception ex)
	    {
		    error = ex;
		    return false;
	    }
    }

    static void WriteSectionToStream(IConfigurationSection section, TextWriter stream)
    {
        if (section.Value != default)
        {
            if (section.Value == "null" ||
                double.TryParse(
	                section.Value,
	                NumberStyles.AllowLeadingSign
		                | NumberStyles.AllowExponent
		                | NumberStyles.AllowDecimalPoint,
	                CultureInfo.InvariantCulture, out var _) ||
                bool.TryParse(section.Value, out var _))
            {
                stream.Write(section.Value.ToLower(CultureInfo.InvariantCulture));
                return;
            }
            
            stream.Write('"');
            stream.Write(section.Value);
            stream.Write('"');
            return;
        }

        var children = section.GetChildren().ToList();
        if (!children.Any())
        {
            stream.Write("{}");
            return;
        }

        var isArray = children
	        .Select(_ => _.Key)
	        .All(_ => int.TryParse(_, NumberStyles.None, CultureInfo.InvariantCulture, out var _));

        if (isArray)
        {
            children = children.OrderBy(_ => int.Parse(_.Key, CultureInfo.InvariantCulture)).ToList();
            stream.Write('[');
        }
        else
        {
            stream.Write('{');
        }
        var first = true;
        foreach (var child in children)
        {
            if (!first)
            {
                stream.Write(',');
            }

            if (child.Value == default && !child.GetChildren().Any())
            {
                continue;
            }

            if (!isArray)
            {
                stream.Write('"');
                stream.Write(child.Key);
                stream.Write('"');
                stream.Write(':');
            }
            
            WriteSectionToStream(child, stream);
            first = false;
        }

        stream.Write(isArray ? ']' : '}');
    }
}
