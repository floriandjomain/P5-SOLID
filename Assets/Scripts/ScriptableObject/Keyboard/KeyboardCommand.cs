using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Command/Keyboard")]
public class KeyboardCommand : InputCommand
{
    [SerializeField] private string _name;
    private KeyCode _command;

    public string CommandName { get => _name; }
    public KeyCode Command { get => _command; set => _command = value; }
}
