using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveUpCommand : GameCommand
{
    public MoveUpCommand(string player)
    {
        _player = player;
    }

    public override void Execute()
    {
        GameManager.Instance.SetMovement(_player, Movement.Up);
    }
}
