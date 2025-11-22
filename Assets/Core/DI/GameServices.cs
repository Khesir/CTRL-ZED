/// <summary>
/// Static accessor for the game's DI container.
/// Use this as the single entry point for resolving dependencies.
/// </summary>
public static class GameServices
{
    private static DIContainer _container;

    public static DIContainer Container
    {
        get
        {
            if (_container == null)
            {
                _container = new DIContainer();
            }
            return _container;
        }
    }

    /// <summary>
    /// Initialize the container (call from composition root).
    /// </summary>
    public static void Initialize(DIContainer container)
    {
        _container = container;
    }

    /// <summary>
    /// Resolve a service.
    /// </summary>
    public static T Get<T>() where T : class => Container.Resolve<T>();

    /// <summary>
    /// Try to resolve a service, returns null if not found.
    /// </summary>
    public static T TryGet<T>() where T : class => Container.TryResolve<T>();

    /// <summary>
    /// Check if a service is registered.
    /// </summary>
    public static bool Has<T>() where T : class => Container.IsRegistered<T>();

    /// <summary>
    /// Clear the container (for testing or scene transitions).
    /// </summary>
    public static void Clear()
    {
        _container?.Clear();
        _container = null;
    }
}
