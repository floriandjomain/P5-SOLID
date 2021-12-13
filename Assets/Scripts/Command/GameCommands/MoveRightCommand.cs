using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveRightCommand : GameCommand
{
    public MoveRightCommand(string player)
    {
        _player = player;
    }

    public override void Execute()
    {
        GameManager.Instance.SetMovement(_player, Movement.Right);
    }
}
