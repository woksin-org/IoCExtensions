// Copyright (c) woksin-org. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Reflection;

namespace IoCExtensions.Registry.Types;

/// <summary>
/// Extensions on <see cref="Type"/> for getting information about generic types.
/// </summary>
static class GenericTypeExtensions
{
    /// <summary>
    /// Tries to get the generic type of an implemented generic interface.
    /// </summary>
    /// <param name="type">The <see cref="Type"/> to get the implemented generic interface type from.</param>
    /// <param name="openGenericInterface">The open generic interface.</param>
    /// <returns>The generic type of an implemented generic interface.</returns>
    public static Type GetImplementedGenericInterfaceFirstGenericArgumentType(this Type type, Type openGenericInterface)
    {
        var implementedInterfaces = type
            .GetTypeInfo()
            .ImplementedInterfaces
            .Where(IsGenericInterface(openGenericInterface)).ToArray();

        switch (implementedInterfaces.Length)
        {
            case 0:
                throw new TypeDoesNotImplementGenericInterface(type, openGenericInterface);
            case > 1:
                throw new TypeImplementsGenericInterfaceMultipleTimes(type, openGenericInterface);
        }

        var implementedInterface = implementedInterfaces[0];
        var implementedInterfaceGenericArguments = implementedInterface.GetGenericArguments();
        return implementedInterfaceGenericArguments[0];
    }

    static Func<Type, bool> IsGenericInterface(Type genericInterface)
        => type => type.IsGenericType && type.GetGenericTypeDefinition() == genericInterface;
}
