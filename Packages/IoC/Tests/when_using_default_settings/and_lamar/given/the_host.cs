// Copyright (c) woksin-org. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using IoCExtensions.given;
using IoCExtensions.Lamar;

namespace IoCExtensions.when_using_default_settings.and_lamar.given;

public class the_host : a_host_builder
{
	void Establish()
	{
		host_builder.UseLamarIoCExtensions(entry_assembly);
		BuildHost();
	}
}
