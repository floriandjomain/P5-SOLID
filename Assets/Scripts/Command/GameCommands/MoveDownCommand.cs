using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveDownCommand : GameCommand
{
    public MoveDownCommand(string player)
    {
        _player = player;
    }

    public override void Execute()
    {
        GameManager.Instance.SetMovement(_player, Movement.Down);
    }
}
