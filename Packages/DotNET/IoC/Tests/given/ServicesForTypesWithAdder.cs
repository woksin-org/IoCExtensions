// Copyright (c) woksin-org. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Autofac;
using Microsoft.Extensions.DependencyInjection;

namespace IoCExtensions.given;

public class AutofacServicesForTypesWithAdder : IoCExtensions.Autofac.ICanAddServicesForTypesWith<SomeAttribute>
{
	public void AddServiceFor(Type type, SomeAttribute attribute, ContainerBuilder services)
	{
		services.RegisterType<ClassWithSomeAttribute>().AsSelf();
	}
}

public class ServiceCollectionServicesForTypesWithAdder : ICanAddServicesForTypesWith<AddedByServiceCollectionAdderAttribute, IServiceCollection>
{
	public void AddServiceFor(Type type, AddedByServiceCollectionAdderAttribute attribute, IServiceCollection services)
	{
		services.AddTransient<ClassWithAddedByServiceCollectionAdderAttribute>();
	}
}

public class MicrosoftServicesForTypesWithAdder : ICanAddServicesForTypesWith<SomeAttribute>
{
	public void AddServiceFor(Type type, SomeAttribute attribute, IServiceCollection services)
	{
		services.AddTransient<ClassWithSomeAttribute>();
	}
}



[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class SomeAttribute : Attribute
{

}

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class AddedByServiceCollectionAdderAttribute : Attribute
{

}

[Some]
public class ClassWithSomeAttribute
{

}


[AddedByServiceCollectionAdder]
public class ClassWithAddedByServiceCollectionAdderAttribute
{

}

