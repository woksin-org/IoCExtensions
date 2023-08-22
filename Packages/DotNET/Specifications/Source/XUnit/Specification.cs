// Copyright (c) woksin-org. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Xunit;

namespace Woksin.Extensions.Specifications.XUnit;

/// <summary>
/// Represents the base class for specifications.
/// </summary>
public abstract class Specification : SpecificationBase<Specification>, IAsyncLifetime
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Specification"/> class.
    /// </summary>
    protected Specification()
    {
    }
    
    /// <summary>
    /// Initializes a new instance of the <see cref="Specification"/> class.
    /// </summary>
    /// <param name="arrangeMethodName">The name of the methods used for arranging the test.</param>
    /// <param name="actMethodName">The name of the methods used for acting the test.</param>
    /// <param name="cleanupMethodName">The name of the methods used for cleaning up after a test.</param>
    protected Specification(string arrangeMethodName, string actMethodName, string cleanupMethodName)
		: base(arrangeMethodName, actMethodName, cleanupMethodName)
    {
    }

    /// <inheritdoc/>
    public async Task InitializeAsync()
    {
        await Arrange();
        await Act();
    }

    /// <inheritdoc/>
    public async Task DisposeAsync() => await Cleanup();
}
