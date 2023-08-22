// Copyright (c) woksin-org. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Reflection;

namespace Woksin.Extensions.Specifications;

/// <summary>
/// Represents the base class for specifications.
/// </summary>
/// <typeparam name="TSpecification">The type of the specification class.</typeparam>
public abstract class SpecificationBase<TSpecification>
{
    readonly string _arrangeMethodName;
    readonly string _actMethodName;
    readonly string _cleanupMethodName;

	/// <summary>
	/// Initializes a new instance of the <see cref="SpecificationBase{TSpecification}"/> class.
	/// </summary>
	/// <param name="arrangeMethodName">The name of the methods used for arranging the test.</param>
	/// <param name="actMethodName">The name of the methods used for acting the test.</param>
	/// <param name="cleanupMethodName">The name of the methods used for cleaning up after a test.</param>
	protected SpecificationBase(
		string arrangeMethodName = "Establish",
		string actMethodName = "Because",
		string cleanupMethodName = "Destroy")
	{
		_arrangeMethodName = arrangeMethodName;
		_actMethodName = actMethodName;
		_cleanupMethodName = cleanupMethodName;
	}
	
	/// <summary>
	/// Calls all methods in the class hierarchy for the Arrange-part.
	/// </summary>
	/// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
	protected Task Arrange() => InvokeMethod("Establish", _arrangeMethodName)!;

	/// <summary>
	/// Calls all methods in the class hierarchy for the Act-part.
	/// </summary>
	/// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
	protected Task Act() => InvokeMethod("Because", _actMethodName)!;
	
	/// <summary>
	/// Calls all methods in the class hierarchy for the Cleanup-part.
	/// </summary>
	/// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
	protected Task Cleanup() => InvokeMethod("Destroy", _cleanupMethodName)!;

    Task? InvokeMethod(string reflectionMethodName, string name) => typeof(SpecificationMethods<,>)
	    .MakeGenericType(GetType(), typeof(TSpecification))
	    .GetMethod(reflectionMethodName, BindingFlags.Static | BindingFlags.Public)
	    ?.Invoke(null, new object[] { this, name }) as Task;
}
