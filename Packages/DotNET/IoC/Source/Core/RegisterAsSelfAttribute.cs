// Copyright (c) woksin-org. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Woksin.Extensions.IoC;

/// <summary>
/// Indicates that the class should be registered as itself in a DI container.
/// </summary>
[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class RegisterAsSelfAttribute : Attribute
{
}
