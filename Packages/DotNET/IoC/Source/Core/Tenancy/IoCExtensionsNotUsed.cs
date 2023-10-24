namespace Woksin.Extensions.IoC.Tenancy;

/// <summary>
/// Exception that gets thrown when no IoC Extension is being used.
/// </summary>
public class IoCExtensionsNotUsed : Exception
{
    public IoCExtensionsNotUsed(string underlyingErrorMessage)
        : base($"{underlyingErrorMessage}. It does not seem like any IoC Extension is in use. Please use the IoC Extension for the dependency injection framework you want to use.")
    {
    }
}