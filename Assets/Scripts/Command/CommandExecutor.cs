using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandExecutor : MonoBehaviour
{
    public CommandQueue CommandQueue;

    public void DequeueMessage()
    {
        CommandQueue.Dequeue().Execute();
    }
}
