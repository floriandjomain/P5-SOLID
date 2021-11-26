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

    public void SetLobbyListeningState()
    {
        _state = State.LobbyListening;
    }

    public void SetGameListeningState()
    {
        _state = State.GameListening;
    }

    public void SetNotListeningState()
    {
        _state = State.NotListening;
    }

    public void SetOnPlayState()
    {
        _state = State.OnPlay;
    }

    public State GetState()
    {
        return _state;
    }
}
