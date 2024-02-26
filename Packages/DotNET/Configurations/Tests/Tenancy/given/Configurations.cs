// Copyright (c) woksin-org. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Woksin.Extensions.Configurations.Tenancy.given;

[Configuration("ConfigWithOnePrefix")]
public class ConfigWithOnePrefix
{
	public int SomeInt { get; set; }
}

[TenantConfiguration("ConfigWithOnePrefix")]
public class TenantConfigWithOnePrefix
{
	public int SomeInt { get; set; }
}

[Configuration("ConfigWith", "TwoPrefixes")]
public class ConfigWithTwoPrefixes
{
	public int SomeInt { get; set; }
}
[TenantConfiguration("ConfigWith", "TwoPrefixes")]
public class TenantConfigWithTwoPrefixes
{
	public int SomeInt { get; set; }
}

[Configuration("ConfigWithComplexObject")]
public class ConfigWithComplexObject
{
	public int SomeInt { get; set; }
	public SomeComplex SomeComplex { get; set; }
}
[TenantConfiguration("ConfigWithComplexObject")]
public class TenantConfigWithComplexObject
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

[TenantConfiguration("ConfigWithNestedConfiguration")]
public class TenantConfigWithNestedConfiguration
{
	public int SomeInt { get; set; }
	public TenantNestedConfig SomeNestedConfig { get; set; }
}

[Configuration("ConfigWithNestedConfiguration", "SomeNestedConfig")]
public class NestedConfig
{
	public int SomeNestedInt { get; set; }
}
[TenantConfiguration("ConfigWithNestedConfiguration", "SomeNestedConfig")]
public class TenantNestedConfig
{
	public int SomeNestedInt { get; set; }
}
