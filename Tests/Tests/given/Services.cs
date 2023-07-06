using IoCExtensions.Lifetime;
using Microsoft.Extensions.DependencyInjection;

namespace IoCExtensions.given;

public interface ISingletonService
{
}

public interface IScopedService
{
}

public interface ITransientService
{
}
	
public interface IServiceWithoutLifetimeAttribute
{
}

public class ServiceWithoutLifetimeAttribute : IServiceWithoutLifetimeAttribute
{
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

[WithLifetime(ServiceLifetime.Scoped)]
public class ScopedService : IScopedService
{
}

[WithLifetime(ServiceLifetime.Transient)]
public class TransientService : ITransientService
{
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