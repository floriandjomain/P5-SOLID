using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveUpCommand : GameCommand
{
    public MoveUpCommand(string _player)
    {
        player = _player;
    }

    public override void Execute()
    {
        GameManager.Instance.SetMovement(player, Movement.Up);
    }
}
