using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwitchInterpreter : MonoBehaviour
{
    public Chatline twitchQueue;
    public List<TwitchCommand> lobbyTwitchCommands;
    public List<TwitchCommand> gameTwitchCommands;
    public GameState gameState;

    private void Update()
    {
        if (gameState.GetState() == GameState.State.GameListening || gameState.GetState() == GameState.State.LobbyListening)
        {
            if (twitchQueue.QueueCount() > 0)
            {
                Debug.Log("Dequeue message");
                Interpret(twitchQueue.Dequeue());
            }
        }
    }

    private void Interpret(TwitchChatMessage twitchChatMessage)
    {
        if (gameState.GetState() == GameState.State.GameListening)
        {
            SearchList(twitchChatMessage, gameTwitchCommands);
        }
        else if(gameState.GetState() == GameState.State.LobbyListening)
        {
            SearchList(twitchChatMessage, lobbyTwitchCommands);
        }
    }

    private void SearchList(TwitchChatMessage twitchChatMessage, List<TwitchCommand> twitchCommands)
    {
        foreach (TwitchCommand twitchCommand in twitchCommands)
        {
            if (twitchChatMessage.Message.StartsWith("!" + twitchCommand.GetCommandName()))
            {
                twitchCommand.OnCommandFound(twitchChatMessage.Sender);
                break;
            }
        }
    }
}
