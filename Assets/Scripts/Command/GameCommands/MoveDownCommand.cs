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
        GameManager.Instance.SetMovement(player, Movement.Down);
    }
}
