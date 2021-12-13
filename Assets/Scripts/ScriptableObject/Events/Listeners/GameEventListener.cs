using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameEventListener : MonoBehaviour, IGameEvent
{
    [SerializeField] private GameEvent _event;
    [SerializeField] private UnityEvent _onEventRaised;

    public void OnEventRaised()
    {
        Debug.Log("toto");
        Debug.Log("Listener - " + _onEventRaised.GetPersistentMethodName(0));
        _onEventRaised.Invoke();
        Debug.Log("toto 2");
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
