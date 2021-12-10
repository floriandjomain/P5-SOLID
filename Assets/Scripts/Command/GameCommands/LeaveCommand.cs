using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaveCommand : GameCommand
{
    public LeaveCommand(string player)
    {
        _player = player;
    }

    public override void Execute()
    {
        LobbyManager.Instance.RemovePlayer(_player);
    }
}
