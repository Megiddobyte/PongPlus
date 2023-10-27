using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game Event")]
public class GameEvent : ScriptableObject
{
    private List<GameEventListener> _listeners = new();

    public void Raise(Component sender, object data)
    {
        foreach(var _listener in _listeners)
        {
            _listener.OnEventRaised(sender, data);
        }
    }

    public void RegisterListener(GameEventListener listener)
    {
        if (_listeners.Contains(listener)) return;
        
        _listeners.Add(listener);
    }
    
    public void UnregisterListener(GameEventListener listener)
    {
        if (!_listeners.Contains(listener)) return;

        _listeners.Remove(listener);
    }
}

    
