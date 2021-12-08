using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoinCommand : GameCommand
{
    public JoinCommand(string player)
    {
        _player = player;
    }

    public override void Execute()
    {
        LobbyManager.Instance.AddPlayer(_player);
    }
}
