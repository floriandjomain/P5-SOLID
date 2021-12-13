using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Queue/TwitchChatMessage")]
public class TwitchChatMessageQueue : RuntimeQueue<TwitchChatMessage>
{
    [SerializeField] private ChangeValueEvent _event;
    public override void Enqueue(TwitchChatMessage value)
    {
        base.Enqueue(value);
        _event.Raise();
    }
}
