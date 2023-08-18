// Copyright (c) woksin-org. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Xunit;

namespace Configuration.Extension.when_building_host.using_a_config_prefix.using_appsettings.and_autofac_implementation;

public class when_resolving_configurations : given.the_host
{
	void Because()
	{
		SetupAllServices();
		
	}

	[Fact] protected override void should_get_expected_config_with_one_prefix()
	{
		base.should_get_expected_config_with_one_prefix();
	}

	[Fact] protected override void should_get_expected_config_with_two_prefixes()
	{
		base.should_get_expected_config_with_two_prefixes();
	}

	[Fact] protected override void should_get_expected_config_with_complex_object()
	{
		base.should_get_expected_config_with_complex_object();
	}

	[Fact] protected override void should_get_expected_config_with_nested_configuration()
	{
		base.should_get_expected_config_with_nested_configuration();
	}

	[Fact] protected override void should_get_expected_nested_config()
	{
		base.should_get_expected_nested_config();
	}
}
