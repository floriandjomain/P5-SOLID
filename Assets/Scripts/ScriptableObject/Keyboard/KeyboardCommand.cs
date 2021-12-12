using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Command/Keyboard")]
public class KeyboardCommand : ScriptableObject
{
    public enum CommandType { Lobby, Game }

    [SerializeField] private string _name;
    private KeyCode _command;
    [SerializeField] private CommandType _type;
    [SerializeField] private UnityEvent<string> _onCommandFound;

    public string CommandName { get => _name; }
    public KeyCode Command { get => _command; set => _command = value; }
    public CommandType Type { get => _type; }
    public UnityEvent<string> OnCommandFound { get => _onCommandFound; }
}
