using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandExecutor : MonoBehaviour
{
    [SerializeField] private CommandQueue _commandQueue;

    public void DequeueMessage()
    {
        _commandQueue.Dequeue().Execute();
    }
}
