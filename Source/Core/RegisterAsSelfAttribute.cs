namespace IoCExtensions;

/// <summary>
/// Indicates that the class should be registered as itself in a DI container.
/// </summary>
[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class RegisterAsSelfAttribute : Attribute
{
}
