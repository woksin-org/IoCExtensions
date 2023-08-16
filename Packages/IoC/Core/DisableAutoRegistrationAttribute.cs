namespace IoCExtensions;

/// <summary>
/// Indicates that the class should not be registered automatically in a DI container.
/// </summary>
[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class DisableAutoRegistrationAttribute : Attribute
{
}
