using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GenericEvent<T> : ScriptableObject where T : IGameEvent
{
    private List<T> _eventListeners = new List<T>();

    public void Raise()
    {
        for (int i = _eventListeners.Count - 1; i >= 0; i--)
        {
            _eventListeners[i].OnEventRaised();
        }
    }

    public void RegisterListener(T listener)
    {
        if(!_eventListeners.Contains(listener))
            _eventListeners.Add(listener);
    }

    public void UnregisterListener(T listener)
    {
        if (_eventListeners.Contains(listener))
            _eventListeners.Remove(listener);
    }
}
