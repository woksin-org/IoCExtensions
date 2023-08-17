// Copyright (c) woksin-org. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace IoCExtensions.Registry;

/// <summary>
/// Exception that gets thrown when instantiating a <see cref="Type"/> using the default parameterless constructor fails.
/// </summary>
public class CouldNotCreateInstanceOfType : Exception
{
	/// <summary>
	/// Initializes a new instance of the <see cref="CouldNotCreateInstanceOfType"/> class.
	/// </summary>
	/// <param name="type">The type that could not be instantiated.</param>
	public CouldNotCreateInstanceOfType(Type type)
		: base($"Could not create instance of ${type}, make sure it has a public parameterless constructor.")
	{
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="CouldNotCreateInstanceOfType"/> class.
	/// </summary>
	/// <param name="type">The type that could not be created.</param>
	/// <param name="exception">The inner exception.</param>
	public CouldNotCreateInstanceOfType(Type type, Exception exception)
		: base($"Could not create instance of ${type} because ${exception.Message}, make sure it has a public parameterless constructor.")
	{
	}
}