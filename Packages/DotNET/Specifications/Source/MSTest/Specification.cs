// Copyright (c) woksin-org. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Woksin.Extensions.Specifications.MSTest;

/// <summary>
/// Represents the base class for specifications.
/// </summary>
public abstract class Specification : SpecificationBase<Specification>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Specification"/> class.
    /// </summary>
    public Specification()
    {
    }
    
    /// <summary>
    /// Initializes a new instance of the <see cref="Specification"/> class.
    /// </summary>
    /// <param name="arrangeMethodName">The name of the methods used for arranging the test.</param>
    /// <param name="actMethodName">The name of the methods used for acting the test.</param>
    /// <param name="cleanupMethodName">The name of the methods used for cleaning up after a test.</param>
    public Specification(string arrangeMethodName, string actMethodName, string cleanupMethodName)
		: base(arrangeMethodName, actMethodName, cleanupMethodName)
    {
    }

    /// <summary>
    /// Initializes the test by doing the arrange and act parts.
    /// </summary>
    /// <returns>The <see cref="Task"/> representing the asynchronous operation.</returns>
    [TestInitialize]
    public async Task InitializeAsync()
    {
        await Arrange();
        await Act();
    }
	
    /// <summary>
    /// Tears down the test by doing the cleanup parts.
    /// </summary>
    /// <returns>The <see cref="Task"/> representing the asynchronous operation.</returns>
    [TestCleanup]
    public async Task DisposeAsync() => await Cleanup();
}
