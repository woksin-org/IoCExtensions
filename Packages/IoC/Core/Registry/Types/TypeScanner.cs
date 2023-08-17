// Copyright (c) woksin-org. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Reflection;
using BaselineTypeDiscovery;
namespace IoCExtensions.Registry.Types;

/// <summary>
/// Represents a system that can scan all Runtime assemblies for classes to be registered in a DI container.
/// </summary>
static class TypeScanner
{
    /// <summary>
    /// Scans all the Runtime assemblies to find exported classes.
    /// </summary>
    /// <returns>An <see cref="IEnumerable{T}"/> of <see cref="Type"/> with all classes in Runtime assemblies.</returns>
    public static IEnumerable<Type> GetAllExportedTypesInRuntimeAssemblies(IoCExtensionsOptions options, out IEnumerable<Assembly> assemblies)
    {
	    var assembliesUsed = new List<Assembly>();
	    assembliesUsed.AddRange(AssemblyFinder.FindAssemblies(
            _ => { },
            assembly => !string.IsNullOrEmpty(assembly.FullName) && assembly.FullName.StartsWith(options.AssemblySearchNamePrefix, StringComparison.InvariantCulture)
                        && options.IgnoredAssemblies.All(ignoredAssembly => assembly != ignoredAssembly)
                        && options.IgnoredAssemblyNames.All(_ => !assembly.FullName.StartsWith(_, StringComparison.InvariantCulture)),
            true));
        assembliesUsed.AddRange(options.AdditionalAssemblies);
        assemblies = assembliesUsed;
        return assemblies.SelectMany(_ => _.ExportedTypes).Where(_ => _ is { IsClass: true, IsAbstract: false });
    }
}
