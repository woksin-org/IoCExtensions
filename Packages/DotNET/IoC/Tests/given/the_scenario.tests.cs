// Copyright (c) woksin-org. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Woksin.Extensions.IoC.given;

public partial class the_scenario
{
	protected virtual void should_get_singleton_service()
	{
		var service = singleton;
		AssertCorrectType<ISingletonService, SingletonService>(service);
		AssertCorrectLifetime(service, ServiceLifetime.Singleton);
	}
	protected virtual void should_get_scoped_service()
	{
		var service = scoped;
		AssertCorrectType<IScopedService, ScopedService>(service);
		AssertCorrectLifetime(service, ServiceLifetime.Scoped);
	}
	protected virtual void should_get_transient_service()
	{
		var service = transient;
		AssertCorrectType<ITransientService, TransientService>(service);
		AssertCorrectLifetime(service, ServiceLifetime.Transient);
	}
    
	protected virtual void should_get_service_without_lifetime()
	{
		var service = service_without_lifetime_attribute;
		AssertCorrectType<IServiceWithoutLifetimeAttribute,ServiceWithoutLifetimeAttribute>(service);
		AssertDefaultLifetime(service);
	}

	protected virtual void should_get_explicitly_added_service()
	{
		var service = explicitly_added_service;
		AssertCorrectType(service);
		AssertCorrectLifetime(service, ServiceLifetime.Transient);
	}

	protected virtual void should_get_service_added_by_service_collection_adder()
	{
		var service = service_added_by_service_collection_adder;
		AssertCorrectType(service);
		AssertCorrectLifetime(service, ServiceLifetime.Transient);
	}
	
	protected virtual void should_get_service_with_some_attribute()
	{
		var service = service_with_some_attribute;
		AssertCorrectType(service);
		AssertCorrectLifetime(service, ServiceLifetime.Transient);
	}
	
	protected virtual void should_get_service_with_some_attribute_added_by_service_callection_adder()
	{
		var service = service_with_some_attribute_added_by_service_callection_adder;
		AssertCorrectType(service);
		AssertCorrectLifetime(service, ServiceLifetime.Transient);
	}
	protected virtual void should_get_service_with_multiple_interfaces()
	{
		var service1 = service_with_multiple_interfaces_1;
		var service2 = service_with_multiple_interfaces_2;
		var service3 = service_with_multiple_interfaces_3;
		var implementationType = typeof(ServiceWithMultipleInterfaces);
		AssertCorrectType(service1, implementationType);
		AssertCorrectType(service2, implementationType);
		AssertCorrectType(service3, implementationType);
		AssertDefaultLifetime(service1);
		AssertDefaultLifetime(service2);
		AssertDefaultLifetime(service3);
	}

	protected virtual void should_get_ternary_generic_service()
	{
		var service = ternary_generic_service;
		var implementationType = typeof(TernaryGenericService<int, string>);
		AssertCorrectType(service, implementationType);
		AssertDefaultLifetime(service);
	}
	protected virtual void should_not_get_not_auto_registered_service()
	{
		var service = not_auto_registered_service;
		Assert.Null(service.First);
		Assert.Null(service.Second);
		Assert.Null(service.FirstScoped);
		Assert.Null(service.SecondScoped);
	}
	
	protected virtual void should_get_transitive_service()
	{
		var serviceBase = transitive_base;
		var service = transitive_service;
		var implementationType = typeof(TransitiveService);
		AssertCorrectType(serviceBase, implementationType);
		AssertCorrectType(service, implementationType);
		AssertDefaultLifetime(serviceBase);
		AssertDefaultLifetime(service);
	}

	protected virtual void should_get_transitive_generic_service()
	{
		var serviceBase = transitive_generic_base;
		var service = transitive_generic_service;
		var implementationType = typeof(TransitiveIntGenericService);
		AssertCorrectType(serviceBase, implementationType);
		AssertCorrectType(service, implementationType);
		AssertDefaultLifetime(serviceBase);
		AssertDefaultLifetime(service);
	}
	
	protected virtual void should_get_transitive_open_generic_service()
	{
		var serviceBase = transitive_open_generic_base;
		var service = transitive_open_generic_service;
		var implementationType = typeof(TransitiveOpenGenericService<string>);
		AssertCorrectType(serviceBase, implementationType);
		AssertCorrectType(service, implementationType);
		AssertDefaultLifetime(serviceBase);
		AssertDefaultLifetime(service);
	}

	protected virtual void should_get_partially_closed_generic_service()
	{
		var service = partially_closed_generic_service;
		var implementationType = typeof(PartiallyClosedGenericService<string>);
		AssertCorrectType(service, implementationType);
		AssertDefaultLifetime(service);
	}
	
	protected virtual void should_get_self_registered_class()
	{
		var service = self_registered_class;
		var implementationType = typeof(SelfRegisteredClass);
		AssertCorrectType(service, implementationType);
		AssertDefaultLifetime(service);
	}
	protected virtual void should_get_self_registered_generic_class()
	{
		var service = self_registered_generic_class;
		var implementationType = typeof(SelfRegisteredGenericClass<int>);
		AssertCorrectType(service, implementationType);
		AssertDefaultLifetime(service);
	}
	
	protected virtual void should_get_self_registered_class_with_scoped_lifetime()
	{
		var service = self_registered_generic_with_scoped_lifetime;
		var implementationType = typeof(SelfRegisteredClassWithScopedLifetime);
		AssertCorrectType(service, implementationType);
		AssertCorrectLifetime(service, ServiceLifetime.Scoped);
	}
	protected void AssertCorrectType<TService, TImplementation>(Services<TService> service)
		=> AssertCorrectType(service, typeof(TImplementation));
	protected void AssertCorrectType<TImplementation>(Services<TImplementation> service)
		=> AssertCorrectType(service, typeof(TImplementation));

	protected void AssertCorrectType<TService>(Services<TService> service, Type implementationType)
	{
		Assert.IsType(implementationType, service.First);
		Assert.IsType(implementationType, service.Second);
		Assert.IsType(implementationType, service.FirstScoped);
		Assert.IsType(implementationType, service.SecondScoped);
	}

	protected void AssertDefaultLifetime<T>(Services<T> service)
		=> AssertCorrectLifetime(service, default_service_lifetime); 
	protected void AssertCorrectLifetime<T>(Services<T> service, ServiceLifetime lifetime)
	{
		switch (lifetime)
		{
			case ServiceLifetime.Singleton:
				Assert.Equal(service.First, service.Second);
				Assert.Equal(service.First, service.FirstScoped);
				Assert.Equal(service.First, service.SecondScoped);
				break;
			case ServiceLifetime.Scoped:
				Assert.Equal(service.First, service.Second);
				Assert.NotEqual(service.First, service.FirstScoped);
				Assert.NotEqual(service.FirstScoped, service.SecondScoped);
				break;
			case ServiceLifetime.Transient:
			default:
				Assert.NotEqual(service.First!, service.Second);
				Assert.NotEqual(service.First!, service.FirstScoped);
				Assert.NotEqual(service.First!, service.SecondScoped);
				Assert.NotEqual(service.FirstScoped!, service.SecondScoped);
				break;
		}
	}
}
