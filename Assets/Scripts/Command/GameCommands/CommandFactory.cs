using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CommandFactory
{
    public static GameCommand GetMoveUpCommand(string player)
    {
        return new MoveUpCommand(player);
    }
    public static GameCommand GetMoveRightCommand(string player)
    {
        return new MoveRightCommand(player);
    }
    public static GameCommand GetMoveDownCommand(string player)
    {
        return new MoveDownCommand(player);
    }
    public static GameCommand GetMoveLeftCommand(string player)
    {
        return new MoveLeftCommand(player);
    }

    public static GameCommand GetJoinCommand(string player)
    {
        return new JoinCommand(player);
    }
    public static GameCommand GetLeaveCommand(string player)
    {
        return new LeaveCommand(player);
    }
}