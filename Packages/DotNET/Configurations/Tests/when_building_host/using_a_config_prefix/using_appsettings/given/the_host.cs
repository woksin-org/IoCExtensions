// Copyright (c) woksin-org. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Extensions.Configuration;

namespace Woksin.Extensions.Configurations.when_building_host.using_a_config_prefix.using_appsettings.given;

public class the_host : using_a_config_prefix.given.a_host_builder
{
	void Establish()
	{
		host_builder.ConfigureAppConfiguration((x, y) => y.AddJsonFile("appsettings_with_prefix.json"));
        BuildHost();
	}
}
