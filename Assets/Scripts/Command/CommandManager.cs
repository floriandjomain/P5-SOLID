using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandManager : MonoBehaviour
{
    public CommandQueue commandQueue;

    private void Update()
    {
        if (commandQueue.QueueCount() > 0) commandQueue.Dequeue().Execute();
    }
}
