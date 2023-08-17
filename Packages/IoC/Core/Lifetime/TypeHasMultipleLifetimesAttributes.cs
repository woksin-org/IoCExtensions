// Copyright (c) woksin-org. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace IoCExtensions.Lifetime;

/// <summary>
/// Exception that gets thrown when a <see cref="Type"/> has multiple lifetimes.
/// </summary>
public class TypeHasMultipleLifetimesAttributes : Exception
{
	/// <summary>
	/// Initializes a new instance of the <see cref="TypeHasMultipleLifetimesAttributes"/> class.
	/// </summary>
	/// <param name="type">The type that has multiple <see cref="WithLifetimeAttribute"/>.</param>
    public TypeHasMultipleLifetimesAttributes(Type type)
        : base($"The type ${type} has multiple lifetime attributes, only one is allowed.")
    {
    }
}
