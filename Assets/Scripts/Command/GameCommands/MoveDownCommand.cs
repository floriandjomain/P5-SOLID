using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveDownCommand : GameCommand
{
    public MoveDownCommand(string _player)
    {
        player = _player;
    }

    public override void Execute()
    {
        Debug.Log("Move Down");
    }
}
