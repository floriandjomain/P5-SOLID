using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TwitchCommand : MonoBehaviour
{
    public enum CommandType { Lobby, Game }

    [SerializeField] public List<string> commands;
    [SerializeField] public CommandType commandType;
    [SerializeField] public UnityEvent<string> onCommandFound;

    public bool Contains(string command)
    {
        foreach(string s in commands)
        {
            if (command == s) return true;
        }
        return false;
    }

}
