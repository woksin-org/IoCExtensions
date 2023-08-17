// Copyright (c) woksin-org. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Extensions.Configuration;

namespace IoCExtensions.Configuration.when_building_host.and_not_using_a_config_prefix.using_appsettings.given;

public class a_host_builder : and_not_using_a_config_prefix.given.a_host_builder
{
	void Establish()
	{
		host_builder.ConfigureAppConfiguration((x, y) => y.AddJsonFile("appsettings_without_prefix.json"));
	}
}
