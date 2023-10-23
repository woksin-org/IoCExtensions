// Copyright (c) woksin-org. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Autofac.Core;
using Autofac.Core.Activators.Delegate;
using Autofac.Core.Lifetime;
using Autofac.Core.Registration;
using Microsoft.Extensions.DependencyInjection;

namespace Woksin.Extensions.IoC.Microsoft.Tenancy;

/// <summary>
/// Represents the <see cref="IRegistrationSource"/> for providing a registration source for unknown services on a tenant container.
/// </summary>
sealed class UnknownServiceOnTenantContainerRegistrationSource : IRegistrationSource
{
    const string MetadataKey = "from-woksin-unknown-registration-source";
    readonly IServiceProvider _rootProvider;
    readonly bool _isTenantRootContainer;

    /// <summary>
    /// Initializes a new instance of the <see cref="UnknownServiceOnTenantContainerRegistrationSource"/> class.
    /// </summary>
    /// <param name="rootProvider">The root <see cref="IServiceProvider"/>.s</param>
    /// <param name="isTenantRootContainer">Whether the registration source belongs to the tenant root container.</param>
    public UnknownServiceOnTenantContainerRegistrationSource(IServiceProvider rootProvider, bool isTenantRootContainer)
    {
        _rootProvider = rootProvider;
        _isTenantRootContainer = isTenantRootContainer;
    }

    internal IComponentRegistry? ParentComponentRegistry { private get; set; }

    /// <inheritdoc />
    public IEnumerable<IComponentRegistration> RegistrationsFor(Service service, Func<Service, IEnumerable<ServiceRegistration>> registrationAccessor)
    {
        if (service is not IServiceWithType serviceWithType)
        {
            return Enumerable.Empty<IComponentRegistration>();
        }

        if (RegisteredInContainer(service, serviceWithType.ServiceType, registrationAccessor) || !IsRegisteredInRootContainer(serviceWithType.ServiceType))
        {
            return Enumerable.Empty<IComponentRegistration>();
        }

        var serviceType = serviceWithType.ServiceType;
        var registration = new ComponentRegistration(
            Guid.NewGuid(),
#pragma warning disable CA2000
            new DelegateActivator(
                serviceType,
                (_, __) => _rootProvider.GetRequiredService(serviceType)),
#pragma warning restore CA2000
            new CurrentScopeLifetime(),
            InstanceSharing.None,
            InstanceOwnership.ExternallyOwned,
            new[]
            {
                service
            },
            new Dictionary<string, object>
            {
                [MetadataKey] = null!
            }!);
        return new[]
        {
            registration
        };
    }

    /// <inheritdoc />
    public bool IsAdapterForIndividualComponents => false;

    bool RegisteredInContainer(Service service, Type serviceType, Func<Service, IEnumerable<ServiceRegistration>> registrationAccessor)
    {
        if (ParentComponentRegistry is null)
        {
            return false;
        }
        foreach (var registration in ParentComponentRegistry.Registrations)
        {
            if (registration.Services.Any(service.Equals))
            {
                return !registration.Metadata.ContainsKey(MetadataKey);
            }
        }
        if (!serviceType.IsGenericType)
        {
            return false;
        }
        foreach (var registrationSource in ParentComponentRegistry.Sources.Where(source => source != this))
        {
            var registrations = registrationSource.RegistrationsFor(service, registrationAccessor);
            if (registrations.Any())
            {
                return true;
            }
        }
        return false;
    }

    bool IsRegisteredInRootContainer(Type service)
    {
        try
        {
            if (!_isTenantRootContainer)
            {
                return _rootProvider.GetService(service) is not null;
            }
            var isService = _rootProvider.GetRequiredService<IServiceProviderIsService>();
            var scopeFactory = _rootProvider.GetService<IServiceScopeFactory>();
            if (scopeFactory is null)
            {
                return _rootProvider.GetService(service) is not null;
            }
            using var scope = scopeFactory.CreateScope();
            return scope.ServiceProvider.GetService(service) is not null;
        }
#pragma warning disable CA1031
        catch
#pragma warning restore CA1031
        {
            return false;
        }
    }
}
