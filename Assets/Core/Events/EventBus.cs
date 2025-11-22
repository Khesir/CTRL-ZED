using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EventBus : IEventBus
{
    private readonly Dictionary<Type, List<Delegate>> _subscribers = new();
    private readonly string _name;

    public EventBus(string name = "EventBus")
    {
        _name = name;
    }

    public void Subscribe<T>(Action<T> handler) where T : struct
    {
        var type = typeof(T);
        if (!_subscribers.ContainsKey(type))
            _subscribers[type] = new List<Delegate>();

        _subscribers[type].Add(handler);
    }

    public void Unsubscribe<T>(Action<T> handler) where T : struct
    {
        var type = typeof(T);
        if (_subscribers.TryGetValue(type, out var handlers))
            handlers.Remove(handler);
    }

    public void Publish<T>(T eventData) where T : struct
    {
        var type = typeof(T);
        if (!_subscribers.TryGetValue(type, out var handlers)) return;

#if UNITY_EDITOR
        Debug.Log($"[{_name}] Publishing {type.Name}");
#endif

        foreach (var handler in handlers.ToList())
            ((Action<T>)handler)?.Invoke(eventData);
    }

    public void Clear()
    {
        _subscribers.Clear();
    }

    public int GetSubscriberCount<T>() where T : struct
    {
        var type = typeof(T);
        return _subscribers.TryGetValue(type, out var handlers) ? handlers.Count : 0;
    }

    public int GetTotalSubscriberCount()
    {
        return _subscribers.Values.Sum(h => h.Count);
    }
}
