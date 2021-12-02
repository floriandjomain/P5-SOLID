using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game state")]
public class GameState : ScriptableObject
{
    public enum State
    {
        LobbyListening,
        GameListening,
        NotListening,
        OnPlay
    }

    [SerializeField] private State _state;

    public State GetState()
    {
        return _state;
    }

    public void SetState(State state)
    {
        _state = state;
    }
}
