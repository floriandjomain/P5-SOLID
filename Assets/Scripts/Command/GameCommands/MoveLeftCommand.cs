using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLeftCommand : GameCommand
{
    public MoveLeftCommand(string player)
    {
        _player = player;
    }

    public override void Execute()
    {
        GameManager.Instance.SetMovement(_player, Movement.Left);
    }
}
