// Copyright (c) woksin-org. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Reflection;
using BaselineTypeDiscovery;

namespace Woksin.Extensions.IoC.Registry.Types;

/// <summary>
/// Represents a system that can scan all Runtime assemblies for classes to be registered in a DI container.
/// </summary>
static class TypeScanner
{
    public static IEnumerable<Type> GetAllExportedTypesInRuntimeAssemblies(IoCSettings settings, out IEnumerable<Assembly> assemblies)
    {
	    var assembliesUsed = new List<Assembly>();
	    assembliesUsed.AddRange(AssemblyFinder.FindAssemblies(
            _ => { },
            assembly => !string.IsNullOrEmpty(assembly.FullName) && assembly.FullName.StartsWith(settings.AssemblySearchNamePrefix, StringComparison.InvariantCulture)
                        && settings.IgnoredAssemblies.All(ignoredAssembly => assembly != ignoredAssembly)
                        && settings.IgnoredAssemblyNames.All(_ => !assembly.FullName.StartsWith(_, StringComparison.InvariantCulture)),
            true));
        assembliesUsed.AddRange(settings.AdditionalAssemblies);
        assemblies = assembliesUsed;
        return assemblies.SelectMany(_ => _.ExportedTypes).Where(_ => _ is { IsClass: true, IsAbstract: false });
    }
}
