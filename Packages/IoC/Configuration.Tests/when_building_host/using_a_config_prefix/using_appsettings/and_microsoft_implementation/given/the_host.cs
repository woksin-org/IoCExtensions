// Copyright (c) woksin-org. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using IoCExtensions.Microsoft;

namespace IoCExtensions.Configuration.when_building_host.using_a_config_prefix.using_appsettings.and_microsoft_implementation.given;

public class the_host : using_appsettings.given.a_host_builder
{
	void Establish()
	{
		host_builder
			.UseMicrosoftIoCExtensions(entry_assembly);
		BuildHost();
	}
}
