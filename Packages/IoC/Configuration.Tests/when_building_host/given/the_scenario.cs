// Copyright (c) woksin-org. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Reflection;
using Aksio.Specifications;
using IoCExtensions.Configuration.given;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace IoCExtensions.Configuration.when_building_host.given;

public partial class the_scenario : Specification
{
	protected IHost? host;
	protected Assembly entry_assembly;
	
	protected IOptions<ConfigWithOnePrefix> config_with_one_prefix;
	protected IOptions<ConfigWithTwoPrefixes> config_with_two_prefixes;
	protected IOptions<ConfigWithComplexObject> config_with_complex_object;
	protected IOptions<ConfigWithNestedConfiguration> config_with_nested_configuration;
	protected IOptions<NestedConfig> nested_config;

	protected ConfigWithOnePrefix expected_config_with_one_prefix;
	protected ConfigWithTwoPrefixes expected_config_with_two_prefixes;
	protected ConfigWithComplexObject expected_config_with_complex_object;
	protected ConfigWithNestedConfiguration expected_config_with_nested_configuration;
	protected NestedConfig expected_nested_config;

	void Establish()
	{
		entry_assembly = typeof(a_host_builder).Assembly;
	}

	void Destroy()
	{
		host?.Dispose();
	}

	protected void SetupAllServices()
	{
		SetService(ref config_with_one_prefix);
		SetService(ref config_with_two_prefixes);
		SetService(ref config_with_complex_object);
		SetService(ref config_with_nested_configuration);
		SetService(ref nested_config);

		expected_config_with_one_prefix = new ConfigWithOnePrefix { SomeInt = 43 };
		expected_config_with_two_prefixes = new ConfigWithTwoPrefixes { SomeInt = 44 };
		expected_config_with_complex_object = new ConfigWithComplexObject
			{ SomeInt = 45, SomeComplex = new SomeComplex { AStringValue = "value" } };
		expected_config_with_nested_configuration = new ConfigWithNestedConfiguration
			{ SomeInt = 46, SomeNestedConfig = new NestedConfig{SomeNestedInt = 47} };
		expected_nested_config = new NestedConfig { SomeNestedInt = 47 };
	}

	// ReSharper disable once RedundantAssignment
	void SetService<T>(ref T services) => services = host!.Services.GetService<T>()!;
}
