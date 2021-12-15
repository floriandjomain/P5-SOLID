using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameEventListenerWithString : MonoBehaviour, IEventRaisedWithString
{
    [SerializeField] private GameEventWithString _event;
    [SerializeField] private UnityEvent<string> _onEventRaised;

    public void OnEventRaised(string _string)
    {
        _onEventRaised.Invoke(_string);
    }

    private void OnEnable()
    {
        _event.RegisterListener(this);
    }

    private void OnDisable()
    {
        _event.UnregisterListener(this);
    }
}
