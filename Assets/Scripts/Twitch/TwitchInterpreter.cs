using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwitchInterpreter : MonoBehaviour
{
    [SerializeField] private TwitchChatMessageQueue _twitchQueue;
    [SerializeField] private GameState _gameState;

    [SerializeField] private GameObject _commands;

    [SerializeField] private char commandPerfix;

    private List<TwitchCommand> _lobbyTwitchCommands;
    private List<TwitchCommand> _gameTwitchCommands;

    private void Start()
    {
        _lobbyTwitchCommands = new List<TwitchCommand>();
        _gameTwitchCommands = new List<TwitchCommand>();

        if (_commands != null)
        {
            TwitchCommand[] commands = _commands.GetComponents<TwitchCommand>();
            foreach (TwitchCommand tc in commands)
            {
                if(tc.commandType == TwitchCommand.CommandType.Game)
                {
                    _gameTwitchCommands.Add(tc);
                }
                else if (tc.commandType == TwitchCommand.CommandType.Lobby)
                {
                    _lobbyTwitchCommands.Add(tc);
                }
            }
        }
        else
        {
            Debug.Log("[TwitchInterpreter] There is no commands");
        }
    }

    private void Update()
    {
        if (_gameState.GetState() == GameState.State.GameListening || _gameState.GetState() == GameState.State.LobbyListening)
        {
            if (_twitchQueue.QueueCount() > 0)
            {
                //Debug.Log("Dequeue message");
                Interpret(_twitchQueue.Dequeue());
            }
        }
    }

    
    private void Interpret(TwitchChatMessage twitchChatMessage)
    {
        if (_gameState.GetState() == GameState.State.GameListening)
        {
            SearchList(twitchChatMessage, _gameTwitchCommands);
        }
        else if(_gameState.GetState() == GameState.State.LobbyListening)
        {
            SearchList(twitchChatMessage, _lobbyTwitchCommands);
        }
    }

    private void SearchList(TwitchChatMessage twitchChatMessage, List<TwitchCommand> twitchCommands)
    {
        if (twitchChatMessage.Message.StartsWith(commandPerfix.ToString()))
        {
            string command = twitchChatMessage.Message.TrimStart(commandPerfix);

            foreach (TwitchCommand twitchCommand in twitchCommands)
            {
                if (twitchCommand.Contains(command))
                {
                    twitchCommand.onCommandFound.Invoke(twitchChatMessage.Sender);
                    break;
                }
            }
        }
    }
}
