using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Twitch/Command")]
public class TwitchCommand : ScriptableObject
{
    public enum CommandType { Lobby, Game }

    [SerializeField] private List<string> _commands;
    [SerializeField] private CommandType _type;
    [SerializeField] private UnityEvent<string> _onCommandFound;

    public CommandType Type { get => _type; }
    public UnityEvent<string> OnCommandFound { get => _onCommandFound; }

    public bool Contains(string command)
    {
        foreach (string s in _commands)
        {
            if (command == s) return true;
        }
        return false;
    }
}
