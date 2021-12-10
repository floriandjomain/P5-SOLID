using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandManager : MonoBehaviour
{
    public CommandQueue CommandQueue;

    private void Update()
    {
        if (CommandQueue.QueueCount() > 0) CommandQueue.Dequeue().Execute();
    }
}
