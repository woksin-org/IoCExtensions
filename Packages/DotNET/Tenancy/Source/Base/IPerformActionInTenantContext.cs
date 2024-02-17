using Woksin.Extensions.Tenancy.Context;

namespace Woksin.Extensions.Tenancy;

/// <summary>
/// Defines a system that can perform an action in the context of a tenant.
/// </summary>
/// <typeparam name="TTenant">The <see cref="Type"/> of the <see cref="ITenantInfo"/>.</typeparam>
public interface IPerformActionInTenantContext<TTenant>
    where TTenant : class, ITenantInfo, new()
{
    Task<TResult> Perform<TResult>(ITenantContext<TTenant> tenant, Func<ITenantContext<TTenant>, TResult> callback);
    Task<TResult> Perform<TResult>(ITenantContext<TTenant> tenant, Func<ITenantContext<TTenant>, Task<TResult>> callback);
    Task Perform<TResult>(ITenantContext<TTenant> tenant, Action<ITenantContext<TTenant>> callback);
    Task Perform<TResult>(ITenantContext<TTenant> tenant, Func<ITenantContext<TTenant>, Task> callback);
}

/// <summary>
/// Defines a system that can perform an action in the context of a tenant.
/// </summary>
public interface IPerformActionInTenantContext
{
    Task<TResult> Perform<TResult>(TenantContext tenant, Func<TenantContext, TResult> callback);
    Task<TResult> Perform<TResult>(TenantContext tenant, Func<TenantContext, Task<TResult>> callback);
    Task Perform<TResult>(TenantContext tenant, Action<TenantContext> callback);
    Task Perform<TResult>(TenantContext tenant, Func<TenantContext, Task> callback);
}
