namespace IoCExtensions.Configuration.given;

[Configuration("ConfigWithOnePrefix")]
public class ConfigWithOnePrefix
{
	public int SomeInt { get; set; }
}
[Configuration("ConfigWith", "TwoPrefixes")]
public class ConfigWithTwoPrefixes
{
	public int SomeInt { get; set; }
}

[Configuration("ConfigWithComplexObject")]
public class ConfigWithComplexObject
{
	public int SomeInt { get; set; }
	public SomeComplex SomeComplex { get; set; }
}

public class SomeComplex
{
	public string AStringValue { get; set; }
}
[Configuration("ConfigWithNestedConfiguration")]
public class ConfigWithNestedConfiguration
{
	public int SomeInt { get; set; }
	public NestedConfig SomeNestedConfig { get; set; }
}

[Configuration("ConfigWithNestedConfiguration", "SomeNestedConfig")]
public class NestedConfig
{
	public int SomeNestedInt { get; set; }
}