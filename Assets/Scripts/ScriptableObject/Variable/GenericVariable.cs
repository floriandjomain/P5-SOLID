using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GenericVariable<T> : ScriptableObject
{
    public T Value;

    /*public void SetValue(object newValue)
    {
        object oldValue = Value;
        Value = newValue;

        for(int i = _observers.Count - 1; i++)
        {

        }
    }

    public void RegisterObserver(GenericVariableObserver observer)
    {

    }

    private List<GenericVariableObserver> _observers = new List<GenericVariableObserver>();*/
}
