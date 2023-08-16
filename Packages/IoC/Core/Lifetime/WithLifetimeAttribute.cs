using Microsoft.Extensions.DependencyInjection;

namespace IoCExtensions.Lifetime;

/// <summary>
/// Indicates the <see cref="ServiceLifetime" />
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class WithLifetimeAttribute : Attribute
{
	/// <summary>
	/// Initializes a new instance of the <see cref="WithLifetimeAttribute"/>-
	/// </summary>
	/// <param name="lifetime">THe <see cref="ServiceLifetime"/> to register the service as.</param>
    public WithLifetimeAttribute(ServiceLifetime lifetime)
    {
        Lifetime = lifetime;
    }

    /// <summary>
    /// Gets the <see cref="ServiceLifetime" />.
    /// </summary>
    /// <value></value>
    public ServiceLifetime Lifetime { get; }
}
