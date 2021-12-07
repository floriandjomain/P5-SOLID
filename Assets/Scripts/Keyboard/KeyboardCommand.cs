using UnityEngine;
using UnityEngine.Events;

public class KeyboardCommand : MonoBehaviour
{
    
    public enum CommandType { Lobby, Game }

    [SerializeField] public string CommandName;
    [SerializeField] public KeyCode Command;
    [SerializeField] public CommandType commandType;
    [SerializeField] public UnityEvent<string> onCommandFound;
}
