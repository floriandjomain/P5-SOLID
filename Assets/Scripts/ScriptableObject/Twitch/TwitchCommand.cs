using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Command/Twitch")]
public class TwitchCommand : InputCommand
{
    [SerializeField] private List<string> _commands;

    public List<string> Commands { get => _commands; }

    public bool Contains(string command)
    {
        foreach (string s in _commands)
        {
            if (command == s) return true;
        }
        return false;
    }
}
