// Copyright (c) woksin-org. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections;
using System.Linq.Expressions;
using Autofac;
using Autofac.Core;
using Autofac.Core.Activators.Delegate;
using Autofac.Core.Lifetime;
using Autofac.Core.Registration;
using Autofac.Features.Decorators;
using Autofac.Util.Cache;

namespace Woksin.Extensions.IoC.Microsoft.Tenancy;

class CollectionRegistrationSource : IRegistrationSource, IPerScopeRegistrationSource
{
    readonly IServiceProvider _rootProvider;

    public CollectionRegistrationSource(IServiceProvider rootProvider)
    {
        _rootProvider = rootProvider;
    }

    /// <summary>
    /// Retrieve registrations for an unregistered service, to be used
    /// by the container.
    /// </summary>
    /// <param name="service">The service that was requested.</param>
    /// <param name="registrationAccessor">A function that will return existing registrations for a service.</param>
    /// <returns>Registrations providing the service.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="service"/> or <paramref name="registrationAccessor"/>is <c>null</c>.</exception>
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "Reliability",
        "CA2000:Dispose objects before losing scope",
        Justification = "Activator lifetime controlled by registry.")]
    public IEnumerable<IComponentRegistration> RegistrationsFor(Service service, Func<Service, IEnumerable<ServiceRegistration>> registrationAccessor)
    {
        if (service == null)
        {
            throw new ArgumentNullException(nameof(service));
        }

        if (registrationAccessor == null)
        {
            throw new ArgumentNullException(nameof(registrationAccessor));
        }

        if (service is not IServiceWithType swt || service is DecoratorService)
        {
            return Enumerable.Empty<IComponentRegistration>();
        }

        var serviceType = swt.ServiceType;
        var factoryCache = ReflectionCacheSet.Shared.GetOrCreateCache<ReflectionCacheDictionary<Type, (Type? ElementType, Type? LimitType, Func<int, IList>? Factory)>>(nameof(CollectionRegistrationSource));
        var (elementType, limitType, factory) = factoryCache.GetOrAdd(serviceType, static serviceType =>
        {
            Type? elementType = null;
            Type? limitType = null;
            Func<int, IList>? factory = null;

            if (IsGenericTypeDefinedBy(serviceType, typeof(IEnumerable<>)))
            {
                elementType = serviceType.GenericTypeArguments[0];
                limitType = elementType.MakeArrayType();
                factory = GenerateArrayFactory(elementType);
            }
            else if (serviceType.IsArray)
            {
                // GetElementType always non-null if IsArray is true.
                elementType = serviceType.GetElementType()!;
                limitType = serviceType;
                factory = GenerateArrayFactory(elementType);
            }
            else if (IsGenericListOrCollectionInterfaceType(serviceType))
            {
                elementType = serviceType.GenericTypeArguments[0];
                limitType = typeof(List<>).MakeGenericType(elementType);
                factory = GenerateListFactory(elementType);
            }

            return (elementType, limitType, factory);
        });

        if (elementType == null || factory == null || limitType == null)
        {
            return Enumerable.Empty<IComponentRegistration>();
        }

        var elementTypeService = swt.ChangeType(elementType);
        var activator = new DelegateActivator(
            limitType,
            (c, p) =>
            {
                var itemRegistrations = c.ComponentRegistry
                    .ServiceRegistrationsFor(elementTypeService)
                    .Where(cr => !cr.Registration.Options.HasOption(RegistrationOptions.ExcludeFromCollections))
                    .OrderBy(cr => GetRegistrationOrder(cr.Registration))
                    .ToList();
                var itemsFromRoot = _rootProvider.GetService(serviceType) as IEnumerable ?? Enumerable.Empty<object>();
                var objectArr = new ArrayList();
                foreach (var item in itemsFromRoot)
                {
                    objectArr.Add(item);
                }
                var output = factory(itemRegistrations.Count + objectArr.Count);
                var isFixedSize = output.IsFixedSize;
                var i = 0;
                for (; i < itemRegistrations.Count; i++)
                {
                    var itemRegistration = itemRegistrations[i];
                    var resolveRequest = new ResolveRequest(elementTypeService, itemRegistration, p);
                    var component = c.ResolveComponent(resolveRequest);
                    if (isFixedSize)
                    {
                        output[i] = component;
                    }
                    else
                    {
                        output.Add(component);
                    }
                }

                foreach (var item in objectArr)
                {
                    if (isFixedSize)
                    {
                        output[i] = item;
                    }
                    else
                    {
                        output.Add(item);
                    }
                    i++;
                }

                return output;
            });

        var registration = new ComponentRegistration(
            Guid.NewGuid(),
            activator,
            CurrentScopeLifetime.Instance,
            InstanceSharing.None,
            InstanceOwnership.ExternallyOwned,
            new[] { service },
            new Dictionary<string, object?>());

        return new IComponentRegistration[] { registration };
    }

    /// <inheritdoc/>
    public bool IsAdapterForIndividualComponents => false;

    static Func<int, IList> GenerateListFactory(Type elementType)
    {
        var parameter = Expression.Parameter(typeof(int));
        var genericType = typeof(List<>).MakeGenericType(elementType);
        var constructor = genericType.GetMatchingConstructor(new[] { typeof(int) });

        // We know that List<> has the constructor we need.
        var newList = Expression.New(constructor!, parameter);
        return Expression.Lambda<Func<int, IList>>(newList, parameter).Compile();
    }

    static Func<int, IList> GenerateArrayFactory(Type elementType)
    {
        var parameter = Expression.Parameter(typeof(int));
        var newArray = Expression.NewArrayBounds(elementType, parameter);
        return Expression.Lambda<Func<int, IList>>(newArray, parameter).Compile();
    }
    static long GetRegistrationOrder(IComponentRegistration registration)
        => registration.Metadata.TryGetValue("__RegistrationOrder", out var value) ? (long)value! : long.MaxValue;

    static bool IsGenericTypeDefinedBy(Type thisType, Type openGeneric) => thisType is { ContainsGenericParameters: false, IsGenericType: true }
        && thisType.GetGenericTypeDefinition() == openGeneric;

    static bool IsGenericListOrCollectionInterfaceType(Type type)
        => IsGenericTypeDefinedBy(type, typeof(IList<>))
            || IsGenericTypeDefinedBy(type, typeof(ICollection<>))
            || IsGenericTypeDefinedBy(type, typeof(IReadOnlyCollection<>))
            || IsGenericTypeDefinedBy(type, typeof(IReadOnlyList<>));
}
