using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuntimeQueue<T> : ScriptableObject
{
    private Queue<T> _queue = new Queue<T>();

    public virtual void Enqueue(T value)
    {
        _queue.Enqueue(value);
    }

    public T Dequeue()
    {
        return _queue.Dequeue();
    }

    public int QueueCount()
    {
        return _queue.Count;
    }
}
