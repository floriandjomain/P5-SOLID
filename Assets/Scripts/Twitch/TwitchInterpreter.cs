using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;


public class TwitchInterpreter : ListCommandByState<TwitchCommand>
{
    private static TwitchInterpreter _instance;
    public static TwitchInterpreter Instance { get => _instance; }
    [SerializeField] private TwitchChatMessageQueue _twitchQueue;

    [SerializeField] private char _commandPerfix;

    private void Awake()
    {
        if (_instance != null && _instance != this) Destroy(gameObject);
        _instance = this;
    }

    private void Update()
    {
        if (_twitchQueue.QueueCount() > 0)
        {
            Debug.Log("Interpret");
            Interpret(_twitchQueue.Dequeue());
        }
    }

    private void Interpret(TwitchChatMessage twitchChatMessage)
    {
        if (_gameState.GetState() == GameState.State.GameListening)
        {
            SearchList(twitchChatMessage, _gameCommands);
        }
        else if(_gameState.GetState() == GameState.State.LobbyListening)
        {
            SearchList(twitchChatMessage, _lobbyCommands);
        }
    }

    private void SearchList(TwitchChatMessage twitchChatMessage, List<TwitchCommand> twitchCommands)
    {
        if (twitchChatMessage.Message.StartsWith(_commandPerfix.ToString()))
        {
            string command = twitchChatMessage.Message.TrimStart(_commandPerfix);

            foreach (TwitchCommand twitchCommand in twitchCommands)
            {
                if (twitchCommand.Contains(command))
                {
                    twitchCommand.OnCommandFound.Invoke(twitchChatMessage.Sender);
                    Debug.Log("Invoke found command");
                    break;
                }
            }
        }
    }

    public string GetAllGameCommands()
    {
        StringBuilder build = new StringBuilder();
        foreach(TwitchCommand tc in _gameCommands)
        {
            foreach(string c in tc.Commands)
            {
                build.Append(" !" + c);
            }
            build.Append(" / ");
        }
        return build.ToString();
    }

    public string GetAllLobbyCommands()
    {
        StringBuilder build = new StringBuilder();
        foreach (TwitchCommand tc in _lobbyCommands)
        {
            foreach (string c in tc.Commands)
            {
                build.Append(" !" + c);
            }
        }
        return build.ToString();
    }
}
