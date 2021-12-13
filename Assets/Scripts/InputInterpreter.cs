using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InputInterpreter<T> : MonoBehaviour
{
    [SerializeField] protected GameState _gameState;

    [SerializeField] protected List<T> _lobbyCommands;
    [SerializeField] protected List<T> _gameCommands;

}
