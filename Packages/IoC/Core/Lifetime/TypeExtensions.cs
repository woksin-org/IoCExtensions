using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;

namespace IoCExtensions.Lifetime;

/// <summary>
/// Extension methods for <see cref="Type"/>.
/// </summary>
public static class TypeExtensions
{
	/// <summary>
	/// Gets the <see cref="ServiceLifetime"/> for the the <see cref="Type"/>.
	/// </summary>
	/// <param name="type">The type to get the service lifetime for.</param>
	/// <param name="fallbackLifetime">The <see cref="ServiceLifetime"/> to fallback to if type did not have <see cref="WithLifetimeAttribute"/>.</param>
	/// <returns>The service lifetime for the type.</returns>
	public static ServiceLifetime GetLifetimeWithFallback(this Type type, ServiceLifetime fallbackLifetime)
    {
        if (!TryGetLifetime(type, out var lifetime))
        {
            lifetime = fallbackLifetime;
        }
        return lifetime.Value;
    }
    
    /// <summary>
    /// Tries to get the <see cref="ServiceLifetime"/> for the the <see cref="Type"/>.
    /// </summary>
    /// <param name="type">The type to get the service lifetime for.</param>
    /// <param name="lifetime">The outputted <see cref="ServiceLifetime"/> if type has <see cref="WithLifetimeAttribute"/>.</param>
    /// <returns>True if type has a decorated lifetime, false if not..</returns>
    public static bool TryGetLifetime(this Type type, [NotNullWhen(true)]out ServiceLifetime? lifetime)
    {
        lifetime = null;
        var lifetimeAttributes = Attribute.GetCustomAttributes(type, typeof(WithLifetimeAttribute));
        if (lifetimeAttributes.Length > 1)
            throw new TypeHasMultipleLifetimesAttributes(type);

        if (lifetimeAttributes.FirstOrDefault() is not WithLifetimeAttribute lifetimeAttribute)
            return false;

        lifetime = lifetimeAttribute.Lifetime;
        return true;
    }
}
