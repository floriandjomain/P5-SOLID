using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TwitchInterpreter : MonoBehaviour
{
    [SerializeField] private TwitchChatMessageQueue _twitchQueue;
    [SerializeField] private GameState _gameState;

    [SerializeField] private List<TwitchCommand> _lobbyTwitchCommands;
    [SerializeField] private List<TwitchCommand> _gameTwitchCommands;

    [SerializeField] private char _commandPerfix;

    public void Update()
    {
        /*
        if (_twitchQueue.QueueCount() > 0)
        {
            Debug.Log("Dequeue message");
            Interpret(_twitchQueue.Dequeue());
        }
        */
    }

    public void DequeueMessage()
    {
        Debug.Log("Interpreter - Dequeue message");
        // Interpret(_twitchQueue.Dequeue());
    }

    private void Interpret(TwitchChatMessage twitchChatMessage)
    {
        Debug.Log("Interpret");
        Debug.Log(_gameState.GetState());

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
}
