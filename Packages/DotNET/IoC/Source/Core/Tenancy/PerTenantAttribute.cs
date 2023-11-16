// Copyright (c) woksin-org. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Woksin.Extensions.IoC.Tenancy;

/// <summary>
/// Indicates that the class should be registered as a per-tenant dependency in a DI container.
/// Meaning that instances will not be shared across execution contexts for different tenants.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public sealed class PerTenantAttribute : Attribute
{
}
