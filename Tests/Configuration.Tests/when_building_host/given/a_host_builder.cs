using Microsoft.Extensions.Hosting;

namespace IoCExtensions.Configuration.when_building_host.given;

public class a_host_builder : the_scenario
{
	protected IHostBuilder host_builder;

	void Establish()
	{
		host_builder = Host.CreateDefaultBuilder();
	}
	protected void BuildHost() => host = host_builder.Build();
}
