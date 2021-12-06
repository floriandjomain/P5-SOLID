using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameEventListener : MonoBehaviour
{
    public GameEvent m_event;
    public UnityEvent m_onEventRaised;
    private void OnEnable()
    {
        m_event.RegisterListener(this);
    }
    private void OnDisable()
    {
        m_event.UnregisterListener(this);
    }
    public void OnEventRaised()
    {
        m_onEventRaised.Invoke();
    }
}
