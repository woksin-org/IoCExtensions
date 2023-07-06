using IoCExtensions.Autofac;
using IoCExtensions.given;

namespace IoCExtensions.when_using_default_settings.and_autofac.given;

public class the_host : a_host_builder
{
	void Establish()
	{
		host_builder.UseAutofacIoCExtensions(entry_assembly);
		BuildHost();
	}
}
