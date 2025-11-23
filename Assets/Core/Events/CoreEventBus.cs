using System;
using UnityEngine;

/// <summary>
/// Persistent EventBus that survives scene transitions.
/// Use for global events: player data, game state, settings, etc.
/// </summary>
public class CoreEventBus : MonoBehaviour
{
    public static CoreEventBus Instance { get; private set; }

    private EventBus _eventBus;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        _eventBus = new EventBus("CoreEventBus");
        Debug.Log("[CoreEventBus] Initialized");
    }

    public static void Subscribe<T>(Action<T> handler) where T : struct
    {
        if (Instance == null)
        {
            Debug.LogWarning($"[CoreEventBus] Instance is null. Cannot subscribe to {typeof(T).Name}. " +
                $"Caller: {new System.Diagnostics.StackTrace().GetFrame(1)?.GetMethod()?.DeclaringType?.Name}");
            return;
        }
        Instance._eventBus.Subscribe(handler);
    }

    public static void Unsubscribe<T>(Action<T> handler) where T : struct
    {
        if (Instance == null) return;
        Instance._eventBus.Unsubscribe(handler);
    }

    public static void Publish<T>(T eventData) where T : struct
    {
        if (Instance == null)
        {
            Debug.LogWarning($"[CoreEventBus] Instance is null. Cannot publish {typeof(T).Name}.");
            return;
        }
        Instance._eventBus.Publish(eventData);
    }

    public static int GetSubscriberCount<T>() where T : struct
    {
        return Instance?._eventBus.GetSubscriberCount<T>() ?? 0;
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            _eventBus.Clear();
            Instance = null;
        }
    }
}
