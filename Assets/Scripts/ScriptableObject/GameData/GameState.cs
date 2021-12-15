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

    public int ToInt()
    {
        return _state switch
        {
            State.LobbyListening => 0,
            State.GameListening => 1,
            State.NotListening => 2,
            State.OnPlay => 3,
            _ => 4
        };
    }

    public static State GetStateFromInt(int state) => State.LobbyListening+state;

    public void SetState(State state)
    {
        _state = state;
    }

    #endregion

    public List<string> AlivePlayers;
}
