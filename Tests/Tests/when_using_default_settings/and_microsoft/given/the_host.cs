using IoCExtensions.given;
using IoCExtensions.Microsoft;

namespace IoCExtensions.when_using_default_settings.and_microsoft.given;

public class the_host : a_host_builder
{
	void Establish()
	{
		host_builder.UseMicrosoftIoCExtensions(
			entry_assembly,
			_ => _.IgnoredBaseTypes.Add(typeof(IPartiallyClosedGenericService<,>)));
		BuildHost();
	}
}
