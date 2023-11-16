// Copyright (c) woksin-org. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Extensions.Hosting;
using Woksin.Extensions.IoC.Microsoft;

namespace Woksin.Extensions.Configurations.Tenancy.when_building_host.given;

public class a_host_builder : the_scenario
{
	protected IHostBuilder host_builder;

	void Establish()
	{
		host_builder = Host.CreateDefaultBuilder();
        host_builder.UseMicrosoftIoC(typeof(a_host_builder).Assembly);
    }
	protected void BuildHost() => host = host_builder.Build();
}
