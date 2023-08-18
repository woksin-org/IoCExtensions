// Copyright (c) woksin-org. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace IoCExtensions;

/// <summary>
/// Indicates that the class should not be registered automatically in a DI container.
/// </summary>
[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class DisableAutoRegistrationAttribute : Attribute
{
}
