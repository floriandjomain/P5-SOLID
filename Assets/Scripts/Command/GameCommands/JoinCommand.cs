using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoinCommand : GameCommand
{
    public JoinCommand(string _player)
    {
        player = _player;
    }

    public override void Execute()
    {
        Debug.Log("Join");
    }
}
