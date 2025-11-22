using System;
using System.Collections.Generic;

/// <summary>
/// Simple dependency injection container for manual DI.
/// Supports singleton and transient registrations.
/// </summary>
public class DIContainer
{
    private readonly Dictionary<Type, object> _singletons = new();
    private readonly Dictionary<Type, Func<DIContainer, object>> _factories = new();

    /// <summary>
    /// Register a singleton instance.
    /// </summary>
    public void RegisterSingleton<T>(T instance) where T : class
    {
        _singletons[typeof(T)] = instance;
    }

    /// <summary>
    /// Register a factory for creating instances.
    /// </summary>
    public void RegisterFactory<T>(Func<DIContainer, T> factory) where T : class
    {
        _factories[typeof(T)] = c => factory(c);
    }

    /// <summary>
    /// Register a type as singleton (created on first resolve).
    /// </summary>
    public void RegisterLazySingleton<TInterface, TImplementation>(Func<DIContainer, TImplementation> factory)
        where TInterface : class
        where TImplementation : class, TInterface
    {
        _factories[typeof(TInterface)] = c =>
        {
            if (!_singletons.ContainsKey(typeof(TInterface)))
            {
                _singletons[typeof(TInterface)] = factory(c);
            }
            return _singletons[typeof(TInterface)];
        };
    }

    /// <summary>
    /// Resolve a dependency.
    /// </summary>
    public T Resolve<T>() where T : class
    {
        var type = typeof(T);

        // Check singletons first
        if (_singletons.TryGetValue(type, out var singleton))
        {
            return (T)singleton;
        }

        // Check factories
        if (_factories.TryGetValue(type, out var factory))
        {
            return (T)factory(this);
        }

        throw new InvalidOperationException($"No registration found for type {type.Name}");
    }

    /// <summary>
    /// Try to resolve a dependency, returns null if not found.
    /// </summary>
    public T TryResolve<T>() where T : class
    {
        try
        {
            return Resolve<T>();
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// Check if a type is registered.
    /// </summary>
    public bool IsRegistered<T>() where T : class
    {
        var type = typeof(T);
        return _singletons.ContainsKey(type) || _factories.ContainsKey(type);
    }

    /// <summary>
    /// Clear all registrations.
    /// </summary>
    public void Clear()
    {
        _singletons.Clear();
        _factories.Clear();
    }
}
