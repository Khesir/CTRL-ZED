/// <summary>
/// Service Locator pattern - provides clean access to DI Container.
/// Acts as a bridge for MonoBehaviours to access backend services.
/// </summary>
public static class ServiceLocator
{
    /// <summary>
    /// Get a service from the DI container.
    /// </summary>
    public static T Get<T>() where T : class
    {
        return GameServices.Container.Resolve<T>();
    }

    /// <summary>
    /// Try to get a service, returns null if not found.
    /// </summary>
    public static T TryGet<T>() where T : class
    {
        return GameServices.Container.TryResolve<T>();
    }

    /// <summary>
    /// Check if a service is registered.
    /// </summary>
    public static bool IsRegistered<T>() where T : class
    {
        return GameServices.Container.IsRegistered<T>();
    }
}
