using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericVariableObserver<T> : ScriptableObject //GenericEvent<T>
{
    [SerializeField] protected T Value;

    /*public T Value {
        get => Value; 
        set
        {
            T oldValue = Value;
            Value = value;
            Raise(oldValue, value);
        }
    }*/
}
