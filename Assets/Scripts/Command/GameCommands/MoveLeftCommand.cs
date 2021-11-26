using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLeftCommand : GameCommand
{
    public MoveLeftCommand(string _player)
    {
        player = _player;
    }

    public override void Execute()
    {
        Debug.Log("Move Left");
    }
}
