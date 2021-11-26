using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameCommand
{
    protected string player;
    public abstract void Execute();

    public string GetPlayer() { return player; }

}
