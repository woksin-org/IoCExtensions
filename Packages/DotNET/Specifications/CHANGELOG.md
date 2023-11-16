# [2.0.0] - 2023-11-16[PR: #25](https://github.com/woksin-org/Woksin.Extensions/pull/25)
## Summary

Adds mulit-tenancy support and build for .NET 8!

### Added

- .NET 8 support
- **Multi-tenancy support** - Adds multi-tenancy support to IoC and Configuration.Tenancy packages
  - `TenantId` type which is simply represented by a string. Meant to uniquely identify a tenant
  - `PerTenant` attribute used in both the IoC and Configuration system to signify a tenant-scoped dependency
  - Tenant-scoped service providers - Each tenant owns its own scoped dependencies with their own lifetimes. Think of it as a completely isolated IoC containers per tenant
  - Tenant-scoped configuration - Allows for resolving configurations for each tenant
  - Automatic JSON serialization for `TenantId` type for both Microsoft MVC and WebAPI resolving the tenant id from a string
  - Automatic serialization of `TenantId` type from IConfiguration (meaning that a configuration class can have TenantId as property resolved from a string)
  - Adds a Middleware that resolved the `TenantId` for each request based on a strategies
    - Default strategy if no custom strategy is configured is to find the `TenantId` in the `Tenant-Id` header in the `HttpRequest`
    - Can provide custom strategies that will be used for resolving `TenantId` from `HttpContext`
    - `TenantId` filters can also be added to filter out tenant ids resolved from the strategies
- `Woksin.Extensions.Configuration.Tenancy` nuget package

### Changed

- Woksin.Extensions.Configuration project structure to support the multi-tenancy model from the IoC package. Split into multiple projects:
  - Woksin.Extensions.Configuration.Core - Provides the common functionality used by the configuration extension packages
  - Woksin.Extensions.Configuration.Base - Is the old configuration system without multi-tenancy support
  - Woksin.Extensions.Configuration.Tenancy  - Adds multi-tenancy support by depending on the IoC system
- Various breaking changes in classes, class names, etc. That should not impact existing applications using these packages


# [1.0.1] - 2023-9-18[PR: #24](https://github.com/woksin-org/Woksin.Extensions/pull/24)
## Summary

Change github workflow permissions


# [1.0.1] - 2023-9-11[PR: #22](https://github.com/woksin-org/Woksin.Extensions/pull/22)
Add test coverage steps to workflow


# [1.0.0] - 2023-8-22[PR: #15](https://github.com/woksin-org/Woksin.Extensions/pull/15)
## Summary

First release of Woksin.Extensions!


