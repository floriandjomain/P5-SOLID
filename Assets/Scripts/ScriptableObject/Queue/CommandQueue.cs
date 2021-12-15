using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Queue/GameCommand")]
public class CommandQueue : RuntimeQueue<GameCommand>
{
    [SerializeField] private GameEvent _event;

    public override void Enqueue(GameCommand value)
    {
        base.Enqueue(value);
        _event.Raise();
    }
}
