using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game state")]
public class GameState : ScriptableObject
{
    #region Enum
    public enum State
    {
        LobbyListening,
        GameListening,
        NotListening,
        OnPlay,
        OnPause
    }

    [SerializeField] private State _state;

    public State GetState()
    {
        return _state;
    }

    public new string ToString()
    {
        return _state switch
        {
            State.LobbyListening => "LobbyListening",
            State.GameListening => "GameListening",
            State.NotListening => "NotListening",
            State.OnPlay => "OnPlay",
            _ => "OnPause"
        };
    }

    public void SetState(State state)
    {
        _state = state;
    }

    #endregion

    public List<string> AlivePlayers;
}
