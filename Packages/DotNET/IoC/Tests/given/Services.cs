// Copyright (c) woksin-org. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Extensions.DependencyInjection;
using Woksin.Extensions.IoC.Tenancy;

namespace Woksin.Extensions.IoC.given;

public interface ISingletonService
{
}
public interface ISingletonPerTenantService
{
    public TenantId Tenant { get;  }
}

public interface IScopedService
{
}
public interface IScopedPerTenantService
{
    public TenantId Tenant { get;  }
}

public interface ITransientService
{
}

public interface ITransientPerTenantService
{
    public TenantId Tenant { get;  }
}

public interface IServiceWithoutLifetimeAttribute
{
}

public interface IPerTenantServiceWithoutLifetimeAttribute
{
    public TenantId Tenant { get;  }
}

public class ServiceWithoutLifetimeAttribute : IServiceWithoutLifetimeAttribute
{
}

[PerTenant]
public class PerTenantServiceWithoutLifetimeAttribute : IPerTenantServiceWithoutLifetimeAttribute
{
    public PerTenantServiceWithoutLifetimeAttribute(TenantId tenant) => Tenant = tenant;
    public TenantId Tenant { get; }
}


public interface IServiceWithMultipleInterface_1
{

}

public interface IServiceWithMultipleInterface_2<T>
{

}

public interface IServiceWithMultipleInterface_3<T1, T2>
{

}

public interface ITernaryGenericService<T1, T2> {}
public class TernaryGenericService<T1, T2> : ITernaryGenericService<T1, T2> {}
	
public interface INotAutoRegisteredService
{
}

public interface ITransitiveBase
{

}

public interface ITransitiveService : ITransitiveBase
{

}

public interface ITransitiveGenericBase<T>
{

}

public interface ITransitiveGenericService<T> : ITransitiveGenericBase<T>
{

}

public interface IPartiallyClosedGenericService<T1, T2>
{
}


[WithLifetime(ServiceLifetime.Singleton)]
public class SingletonService : ISingletonService
{
}

[PerTenant, WithLifetime(ServiceLifetime.Singleton)]
public class PerTenantSingletonService : ISingletonPerTenantService
{
    public PerTenantSingletonService(TenantId tenant)
    {
        Tenant = tenant;
    }

    public TenantId Tenant { get; }
}

[WithLifetime(ServiceLifetime.Scoped)]
public class ScopedService : IScopedService
{
}

[PerTenant, WithLifetime(ServiceLifetime.Scoped)]
public class PerTenantScopedService : IScopedPerTenantService
{
    public PerTenantScopedService(TenantId tenant)
    {
        Tenant = tenant;
    }

    public TenantId Tenant { get; }
}

[WithLifetime(ServiceLifetime.Transient)]
public class TransientService : ITransientService
{
}

[PerTenant, WithLifetime(ServiceLifetime.Transient)]
public class PerTenantTransientService : ITransientPerTenantService
{
    public PerTenantTransientService(TenantId tenant)
    {
        Tenant = tenant;
    }

    public TenantId Tenant { get; }
}


public class ServiceWithMultipleInterfaces : IServiceWithMultipleInterface_1, IServiceWithMultipleInterface_2<int>,
	IServiceWithMultipleInterface_3<int, string>
{
}

[DisableAutoRegistration]
public class NotAutoRegisteredService : INotAutoRegisteredService
{

}

public class TransitiveService : ITransitiveService
{

}

public class TransitiveIntGenericService : ITransitiveGenericService<int>
{
}

public class TransitiveOpenGenericService<T> : ITransitiveGenericService<T>
{
}

[RegisterAsSelf]
public class SelfRegisteredClass
{
}
[PerTenant, RegisterAsSelf]
public class PerTenantSelfRegisteredClass
{
    public PerTenantSelfRegisteredClass(TenantId tenant)
    {
        Tenant = tenant;
    }

    public TenantId Tenant { get; }
}

[RegisterAsSelf]
public class SelfRegisteredGenericClass<T>
{
}



[RegisterAsSelf, WithLifetime(ServiceLifetime.Scoped)]
public class SelfRegisteredClassWithScopedLifetime
{
}

public class PartiallyClosedGenericService<T2> : IPartiallyClosedGenericService<int, T2>
{
	
}
