using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class InputCommand : ScriptableObject
{
    public enum CommandType { Lobby, Game }

    [SerializeField] private UnityEvent<string> _onCommandFound;  
    [SerializeField] private CommandType _type;

    public CommandType Type { get => _type; }
    public UnityEvent<string> OnCommandFound { get => _onCommandFound; }
}
