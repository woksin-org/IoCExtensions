// Copyright (c) woksin-org. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Extensions.Configuration;

namespace Configuration.Extension.when_building_host.and_not_using_a_config_prefix.using_appsettings.given;

public class the_host : and_not_using_a_config_prefix.given.a_host_builder
{
	void Establish()
	{
		host_builder.ConfigureAppConfiguration((x, y) => y.AddJsonFile("appsettings_without_prefix.json"));
        BuildHost();
	}
}
