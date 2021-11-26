using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Queue/GameCommand")]
public class CommandQueue : RuntimeQueue<GameCommand>
{ }
