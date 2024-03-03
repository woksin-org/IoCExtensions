# [1.1.0] - 2024-3-3[PR: #30](https://github.com/woksin-org/Woksin.Extensions/pull/30)
## Summary

Added more features that makes it easier to use this package together with third party libraries and frameworks like Wolverine, Akka, etc.

### Added

- Add resolved tenant context to the `HttpContext.Item` dictionary and a way to easily retrieve it from the `HttpContext` without relying on the `AsyncLocal` `TenantContextAccessor`
- `StaticTenantContextAccessor` that resolves either the statically configured tenant or the 'unresolved' tenant context. This type will be used instead of the `AsyncLocal` `TenantContextAccessor` if `DisableAsyncLocalTenantContext` method on `TenancyBuilder`. This can be useful if you're only interested in using the tenant context by passing it around in messages or using the one that's added to the `HttpContext`. For instance using frameworks and libraries that relies on running different processes on different threads or resuming contexts (Actor frameworks, Wolverine, etc.) then it can be useful to not rely on the `AsyncLocal` `TenantContextAccessor`


# [1.0.1] - 2024-2-26[PR: #28](https://github.com/woksin-org/Woksin.Extensions/pull/28)
## Summary

Try change packageversion


# [1.0.0] - 2024-2-26[PR: #26](https://github.com/woksin-org/Woksin.Extensions/pull/26)
## Summary

Massively changed how tenancy is handled. Tenancy is now not a strictly IoC bound concept


