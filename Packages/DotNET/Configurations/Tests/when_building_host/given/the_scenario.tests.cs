// Copyright (c) woksin-org. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Xunit;

namespace Woksin.Extensions.Configurations.when_building_host.given;

public partial class the_scenario
{
	protected virtual void should_get_expected_config_with_one_prefix() =>
		AssertConfiguration(config_with_one_prefix.Value, expected_config_with_one_prefix);
	
	protected virtual void should_get_expected_config_with_two_prefixes() =>
		AssertConfiguration(config_with_two_prefixes.Value, expected_config_with_two_prefixes);
	
	protected virtual void should_get_expected_config_with_complex_object() =>
		AssertConfiguration(config_with_complex_object.Value, expected_config_with_complex_object);
	
	protected virtual void should_get_expected_config_with_nested_configuration() =>
		AssertConfiguration(config_with_nested_configuration.Value, expected_config_with_nested_configuration);
	
	protected virtual void should_get_expected_nested_config() =>
		AssertConfiguration(nested_config.Value, expected_nested_config);

	protected void AssertConfiguration<T>(T config, T expected)
	{
		AssertCorrectType(config);
		Assert.Equivalent(config, expected);
	}
	
	protected void AssertCorrectType<T>(T service)
		=> AssertCorrectType(service, typeof(T));

	protected void AssertCorrectType<T>(T service, Type implementationType)
	{
		Assert.IsType(implementationType, service);
	}
}