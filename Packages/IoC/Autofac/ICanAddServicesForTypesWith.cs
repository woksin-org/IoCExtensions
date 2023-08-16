using Autofac;

namespace IoCExtensions.Autofac;

/// <inheritdoc />
public interface ICanAddServicesForTypesWith<in TAttribute> : ICanAddServicesForTypesWith<TAttribute, ContainerBuilder>
	where TAttribute : Attribute
{
}