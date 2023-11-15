// Copyright (c) woksin-org. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.ObjectModel;
using System.Reflection;

// ReSharper disable StaticMemberInGenericType

namespace Woksin.Extensions.Specifications;

/// <summary>
/// Represents the lifecycle methods for a <see cref="SpecificationBase{T}"/>.
/// </summary>
/// <typeparam name="TTest">Target type it represents.</typeparam>
/// <typeparam name="TSpecification">The specification base class.</typeparam>
public static class SpecificationMethods<TTest, TSpecification>
{
    static IEnumerable<MethodInfo>? ArrangeMethods { get; set; }

    static IEnumerable<MethodInfo>? ActMethods { get; set; }

    static IEnumerable<MethodInfo>? CleanupMethods { get; set; }

    /// <summary>
    /// Invoke all Establish methods.
    /// </summary>
    /// <param name="unit">Unit to invoke them on.</param>
    /// <param name="methodName">The name of the method.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public static Task Establish(object unit, string methodName) =>
	    InvokeMethods(ArrangeMethods ??= GetMethodsFor(methodName, false), unit);

    /// <summary>
    /// Invoke all Destroy methods.
    /// </summary>
    /// <param name="unit">Unit to invoke them on.</param>
    /// <param name="methodName">The name of the method.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public static Task Destroy(object unit, string methodName) =>
	    InvokeMethods(CleanupMethods ??= GetMethodsFor(methodName, true), unit);

    /// <summary>
    /// Invoke all Because methods.
    /// </summary>
    /// <param name="unit">Unit to invoke them on.</param>
    /// <param name="methodName">The name of the method.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public static Task Because(object unit, string methodName) =>
	    InvokeMethods(ActMethods  ??= GetMethodsFor(methodName, false), unit);

    static async Task InvokeMethods(IEnumerable<MethodInfo> methods, object unit)
    {
        foreach (var method in methods)
        {
            var result = method.Invoke(unit, []);
            if (result is Task taskResult)
            {
                await taskResult;
            }
        }
    }

    static ReadOnlyCollection<MethodInfo> GetMethodsFor(string name, bool derivedFirst)
    {
        var type = typeof(TTest);
        var methods = new List<MethodInfo>();

        while (type != typeof(TSpecification))
        {
            var method = type!.GetMethod(name, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

            if (method != null)
            {
	            if (derivedFirst) methods.Add(method);
	            else methods.Insert(0, method);
            }

            type = type.BaseType;
        }

        return methods.AsReadOnly();
    }
}
