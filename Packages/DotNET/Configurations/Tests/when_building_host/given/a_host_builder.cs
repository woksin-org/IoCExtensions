// Copyright (c) woksin-org. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Extensions.Hosting;

namespace Woksin.Extensions.Configurations.when_building_host.given;

public class a_host_builder : the_scenario
{
	protected IHostBuilder host_builder;

	void Establish()
	{
		host_builder = Host.CreateDefaultBuilder();
	}
	protected void BuildHost() => host = host_builder.Build();
}
