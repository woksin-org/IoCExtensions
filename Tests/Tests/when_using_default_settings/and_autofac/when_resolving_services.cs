using Xunit;

namespace IoCExtensions.when_using_default_settings.and_autofac;

public class when_resolving_services : given.the_host
{
	void Because()
	{
		SetupAllServices();
	}

	[Fact] protected override void should_get_service_with_some_attribute()
	{
		base.should_get_service_with_some_attribute();
	}

	[Fact] protected override void should_get_service_with_some_attribute_added_by_service_callection_adder()
	{
		base.should_get_service_with_some_attribute_added_by_service_callection_adder();
	}

	[Fact] protected override void should_get_explicitly_added_service()
	{
		base.should_get_explicitly_added_service();
	}

	[Fact] protected override void should_get_service_added_by_service_collection_adder()
	{
		base.should_get_service_added_by_service_collection_adder();
	}

	[Fact] protected override void should_get_singleton_service()
	{
		base.should_get_singleton_service();
	}
	
	[Fact] protected override  void should_get_scoped_service()
	{
		base.should_get_scoped_service();
	}

	[Fact] protected override void should_get_transient_service()
	{
		base.should_get_transient_service();
	}

	[Fact] protected override void should_get_service_without_lifetime()
	{
		base.should_get_service_without_lifetime();
	}

	[Fact] protected override void should_get_service_with_multiple_interfaces()
	{
		base.should_get_service_with_multiple_interfaces();
	}

	[Fact] protected override void should_get_ternary_generic_service()
	{
		base.should_get_ternary_generic_service();
	}

	[Fact] protected override void should_not_get_not_auto_registered_service()
	{
		base.should_not_get_not_auto_registered_service();
	}

	[Fact] protected override void should_get_transitive_service()
	{
		base.should_get_transitive_service();
	}

	[Fact] protected override void should_get_transitive_generic_service()
	{
		base.should_get_transitive_generic_service();
	}

	[Fact] protected override void should_get_transitive_open_generic_service()
	{
		base.should_get_transitive_open_generic_service();
	}

	[Fact] protected override void should_get_partially_closed_generic_service()
	{
		base.should_get_partially_closed_generic_service();
	}

	[Fact] protected override void should_get_self_registered_class()
	{
		base.should_get_self_registered_class();
	}

	[Fact] protected override void should_get_self_registered_generic_class()
	{
		base.should_get_self_registered_generic_class();
	}

	[Fact] protected override void should_get_self_registered_class_with_scoped_lifetime()
	{
		base.should_get_self_registered_class_with_scoped_lifetime();
	}
}
