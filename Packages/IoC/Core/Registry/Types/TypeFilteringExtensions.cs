// Copyright (c) woksin-org. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Reflection;

namespace IoCExtensions.Registry.Types;

/// <summary>
/// Extensions on <see cref="IEnumerable{T}"/> of <see cref="Type"/> for type filtering.
/// </summary>
static class TypeFilteringExtensions
{
    /// <summary>
    /// Ignore classes with a specific attribute.
    /// </summary>
    /// <param name="classes">The <see cref="IEnumerable{T}"/> classes to filter.</param>
    /// <typeparam name="TAttribute">The <see cref="Type"/> of the attribute.</typeparam>
    /// <returns>The <see cref="IEnumerable{T}"/> of <see cref="Type"/> without the <typeparamref name="TAttribute"/> attribute.</returns>
    public static IEnumerable<Type> IgnoreClassesWithAttribute<TAttribute>(this IEnumerable<Type> classes)
        where TAttribute : Attribute
        => classes.Where(type => !Attribute.IsDefined(type, typeof(TAttribute)));

    /// <summary>
    /// Filter classes with a specific attribute.
    /// </summary>
    /// <param name="classes">The <see cref="IEnumerable{T}"/> classes to filter.</param>
    /// <param name="classesWithAttribute">Outputted classes with attribute.</param>
    /// <typeparam name="TAttribute">The <see cref="Type"/> of the attribute.</typeparam>
    /// <returns>The <see cref="IEnumerable{T}"/> of <see cref="Type"/> without the <typeparamref name="TAttribute"/> attribute.</returns>
    public static IEnumerable<Type> FilterClassesWithAttribute<TAttribute>(this IEnumerable<Type> classes, out IEnumerable<Type> classesWithAttribute)
        where TAttribute : Attribute
    {
        var groupedByAttribute = classes.ToLookup(type => Attribute.IsDefined(type, typeof(TAttribute)));
        classesWithAttribute = groupedByAttribute[true];
        return groupedByAttribute[false];
    }

    /// <summary>
    /// Filters classes implementing a specific type.
    /// </summary>
    /// <param name="classes">The <see cref="IEnumerable{T}"/> classes to filter.</param>
    /// <param name="implementing">The <see cref="Type"/> that the classes should be filtered on.</param>
    /// <param name="additionalPredicate">The additional predicate.</param>
    /// <param name="classesImplementing">Outputted classes implementing the given <see cref="Type"/>.</param>
    /// <returns>The <see cref="IEnumerable{T}"/> of <see cref="Type"/> not implementing the given <see cref="Type"/>.</returns>
    public static IEnumerable<Type> FilterClassesImplementing(this IEnumerable<Type> classes, Type implementing, Func<Type, bool>? additionalPredicate, out IEnumerable<Type> classesImplementing)
    {
        var predicate = CreateImplementingPredicate(implementing, additionalPredicate);
        var groupedByImplementing = classes.ToLookup(predicate);
        classesImplementing = groupedByImplementing[true];
        return groupedByImplementing[false];
    }

    static Func<Type, bool> CreateImplementingPredicate(Type implementing, Func<Type, bool>? additionalPredicate)
    {
	    additionalPredicate ??= _ => true;
        if (implementing.IsInterface)
        {
            if (implementing.ContainsGenericParameters)
            {
                return type => type.GetTypeInfo().ImplementedInterfaces.Any(_ => _.IsGenericType && _.GetGenericTypeDefinition() == implementing && additionalPredicate(_));
            }

            return type => type.GetTypeInfo().ImplementedInterfaces.Any(_ => _ == implementing && additionalPredicate(_));
        }

        if (implementing.IsGenericType)
        {
            return type =>
            {
                var baseTypes = GetBaseTypes(type);
                return baseTypes
                    .Where(_ => _.IsGenericTypeDefinition)
                    .Any(baseType => baseType.GetGenericTypeDefinition() == implementing);
            };
        }

        return type => GetBaseTypes(type).Any(baseType => baseType == implementing);

    }

    static IEnumerable<Type> GetBaseTypes(Type type)
    {
        var currentBaseType = type.BaseType;
        while (currentBaseType != null)
        {
            yield return currentBaseType;
            currentBaseType = currentBaseType.BaseType;
        }
    }
}
