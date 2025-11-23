using System;
using UnityEngine;

/// <summary>
/// Scene-scoped EventBus that is destroyed when the scene unloads.
/// Use for scene-specific events: gameplay, combat, UI interactions, etc.
/// Automatically cleans up all subscriptions on scene unload - no memory leaks!
/// </summary>
public class SceneEventBus : MonoBehaviour
{
    public static SceneEventBus Instance { get; private set; }

    private EventBus _eventBus;
    private string _sceneName;

    private void Awake()
    {
        // No DontDestroyOnLoad - this dies with the scene
        Instance = this;
        _sceneName = gameObject.scene.name;
        _eventBus = new EventBus($"SceneEventBus ({_sceneName})");
        Debug.Log($"[SceneEventBus] Initialized for scene: {_sceneName}");
    }

    public static void Subscribe<T>(Action<T> handler) where T : struct
    {
        if (Instance == null)
        {
            Debug.LogWarning($"[SceneEventBus] Instance is null. Cannot subscribe to {typeof(T).Name}. " +
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
            Debug.LogWarning($"[SceneEventBus] Instance is null. Cannot publish {typeof(T).Name}.");
            return;
        }
        Instance._eventBus.Publish(eventData);
    }

    public static int GetSubscriberCount<T>() where T : struct
    {
        return Instance?._eventBus.GetSubscriberCount<T>() ?? 0;
    }

    public static int GetTotalSubscriberCount()
    {
        return Instance?._eventBus.GetTotalSubscriberCount() ?? 0;
    }

    private void OnDestroy()
    {
        Debug.Log($"[SceneEventBus] Destroying for scene: {_sceneName}. Clearing {_eventBus.GetTotalSubscriberCount()} subscribers.");
        _eventBus.Clear();

        if (Instance == this)
            Instance = null;
    }
}
