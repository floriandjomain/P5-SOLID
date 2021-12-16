using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/GameEventString")]
public class GameEventWithString : ScriptableObject
{
    private List<GameEventListenerWithString> _eventListeners = new List<GameEventListenerWithString>();

    public void Raise(string _string)
    {
        for (int i = _eventListeners.Count - 1; i >= 0; i--)
        {
            _eventListeners[i].OnEventRaised(_string);
        }
    }

    public void RegisterListener(GameEventListenerWithString listener)
    {
        if (!_eventListeners.Contains(listener))
            _eventListeners.Add(listener);
    }

    public void UnregisterListener(GameEventListenerWithString listener)
    {
        if (_eventListeners.Contains(listener))
            _eventListeners.Remove(listener);
    }
}