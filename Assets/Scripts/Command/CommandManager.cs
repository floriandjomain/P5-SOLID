using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandManager : MonoBehaviour
{
    public CommandQueue commandQueue;
    public GameState gameState;
    [SerializeField] private PlayerManager playerManager;

    private Dictionary<string, GameCommand> gameCommands = new Dictionary<string, GameCommand>();

    private void Update()
    {
        if (commandQueue.QueueCount() > 0)
        {
            Debug.Log("Dequeue command");
            if (gameState.GetState() == GameState.State.GameListening)
            {
                // Should wait to go to "OnPlay" but store the commands into the Dictionnary
                GameCommand gameCommand = commandQueue.Dequeue();
                string player = gameCommand.GetPlayer();
                if (playerManager.Contains(player)) { 

                    if (gameCommands.ContainsKey(player))
                    {
                        gameCommands.Remove(player);
                    }
                    Debug.Log("Store command");
                    // - MoveUp, MoveDown, MoveLeft, MoveRight
                    gameCommands.Add(player, gameCommand);
                }
            }
            else if (gameState.GetState() == GameState.State.LobbyListening)
            {
                Debug.Log("Execute command");
                // Should be executed directly
                // - join, leave
                commandQueue.Dequeue().Execute();
            }
        }

        if(gameState.GetState() == GameState.State.OnPlay)
        {
            foreach(KeyValuePair<string, GameCommand> keyValuePair in gameCommands)
            {
                Debug.Log(keyValuePair.Key + " : " + keyValuePair.Value);
                keyValuePair.Value.Execute();
            }
            gameCommands.Clear();
        }
    }
}
