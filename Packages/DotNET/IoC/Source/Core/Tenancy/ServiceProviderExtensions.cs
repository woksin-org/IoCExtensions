using Microsoft.Extensions.DependencyInjection;

namespace Woksin.Extensions.IoC.Tenancy;

/// <summary>
/// Extension methods for <see cref="IServiceProvider"/>.
/// </summary>
public static class ServiceProviderExtensions
{
    /// <summary>
    /// Gets the tenant scoped <see cref="IServiceProvider"/> to resolve services specific to tenants.
    /// </summary>
    /// <param name="serviceProvider">The root <see cref="IServiceProvider"/>.</param>
    /// <param name="tenantId">The <see cref="TenantId"/>.</param>
    /// <returns>The tenant scoped <see cref="IServiceProvider"/>.</returns>
    /// <exception cref="IoCExtensionsNotUsed">Thrown when no IoC extension is being used.</exception>
    public static IServiceProvider GetTenantScopedProvider(this IServiceProvider serviceProvider, TenantId tenantId)
    {
        var tenantScopedProvider = serviceProvider.GetService<ITenantScopedServiceProviders>() ?? throw new IoCExtensionsNotUsed("Cannot resolve tenant scoped providers");
        return tenantScopedProvider.ForTenant(tenantId);
    }
}