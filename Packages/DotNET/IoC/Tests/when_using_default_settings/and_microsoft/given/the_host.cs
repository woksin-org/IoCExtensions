// Copyright (c) woksin-org. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Woksin.Extensions.IoC.given;
using Woksin.Extensions.IoC.Microsoft;

namespace Woksin.Extensions.IoC.when_using_default_settings.and_microsoft.given;

public class the_host : a_host_builder
{
	void Establish()
	{
		host_builder.UseMicrosoftIoC(
			entry_assembly,
			_ => _.IgnoredBaseTypes.Add(typeof(IPartiallyClosedGenericService<,>)));
		BuildHost();
	}
}
