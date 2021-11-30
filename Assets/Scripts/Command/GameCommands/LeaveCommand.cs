using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaveCommand : GameCommand
{
    public LeaveCommand(string _player)
    {
        player = _player;
    }

    public override void Execute()
    {
        LobbyManager.Instance.RemovePlayer(player);
    }
}
