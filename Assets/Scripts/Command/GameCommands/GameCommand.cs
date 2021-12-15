using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameCommand
{
    protected string _player;
    public abstract void Execute();

}
