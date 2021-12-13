using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "OnCommandFound")]
public class CommandOnFound : ScriptableObject
{
    [SerializeField] private CommandQueue _commandQueue;

    public void CreateJoinCommand(string player)
    {
        _commandQueue.Enqueue(CommandFactory.GetJoinCommand(player));
    }

    public void CreateLeaveCommand(string player)
    {
        _commandQueue.Enqueue(CommandFactory.GetLeaveCommand(player));
    }

    public void CreateMoveUpCommand(string player)
    {
        _commandQueue.Enqueue(CommandFactory.GetMoveUpCommand(player));
    }

    public void CreateMoveRightCommand(string player)
    {
        _commandQueue.Enqueue(CommandFactory.GetMoveRightCommand(player));
    }

    public void CreateMoveDownCommand(string player)
    {
        _commandQueue.Enqueue(CommandFactory.GetMoveDownCommand(player));

    }

    public void CreateMoveLeftCommand(string player)
    {
        _commandQueue.Enqueue(CommandFactory.GetMoveLeftCommand(player));
    }
}
