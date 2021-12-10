using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveRightCommand : GameCommand
{
    public MoveRightCommand(string _player)
    {
        player = _player;
    }

    public override void Execute()
    {
        GameManager.Instance.SetMovement(player, Movement.Right);
    }
}
