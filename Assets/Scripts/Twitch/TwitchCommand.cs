using UnityEngine;
using UnityEngine.Events;

public class TwitchCommand : MonoBehaviour
{
    public enum CommandType { Lobby, Game }

    [SerializeField] public string command;
    [SerializeField] public CommandType commandType;
    [SerializeField] public UnityEvent<string> onCommandFound;

}
