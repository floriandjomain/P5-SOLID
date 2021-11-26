using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


[CreateAssetMenu(menuName = "Command/Twitch")]
public class TwitchCommand : ScriptableObject
{
    [SerializeField] private string _commandName;
    [SerializeField] private CommandQueue _commandQueue;

    private System.Func<string, GameCommand> _function;

    // ---------------------
    // -------- BAD --------
    // ---------------------
    private void OnEnable()
    {
        switch (_commandName)
        {
            case "join":
                _function = CommandFactory.GetJoinCommand;
            break;
            case "leave":
                _function = CommandFactory.GetLeaveCommand;
                break;
            case "up":
                _function = CommandFactory.GetMoveUpCommand;
                break;
            case "right":
                _function = CommandFactory.GetMoveRightCommand;
                break;
            case "down":
                _function = CommandFactory.GetMoveDownCommand;
                break;
            case "left":
                _function = CommandFactory.GetMoveLeftCommand;
                break;
            default:
                Debug.Log("Command " + _commandName + " not found !");
                break;
        }
    }

    public string GetCommandName()
    {
        return _commandName;
    }

    public void OnCommandFound(string player)
    {
        Debug.Log("Found a command");
        _commandQueue.Enqueue(_function.Invoke(player));
    }
}
