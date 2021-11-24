using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RuntimeSet<T> : ScriptableObject
{
    public List<T> Items = new List<T>();

    public void Register(T t)
    {
        if (!Items.Contains(t)) Items.Add(t);
    }

    public void Unregister(T t)
    {
        if (Items.Contains(t)) Items.Remove(t);
    }
}
