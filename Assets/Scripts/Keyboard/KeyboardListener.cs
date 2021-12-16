using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardListener : ListCommandByState<KeyboardCommand>
{
    [SerializeField] private GameSettings _gameSettings;

    [SerializeField] private KeyboardData[] _keyboardDataList;
    [SerializeField] private StringVariable _channelName;

    private KeyboardData _keyboardData;

    private void Start()
    {
        // Keyboard selection
        foreach (KeyboardData keyboardData in _keyboardDataList)
            if (_gameSettings.StreamerControlType == keyboardData.DataName)
                _keyboardData = keyboardData;

        if (!_keyboardData) _keyboardData = _keyboardDataList[0];
        
        // Keyboard name in case joins
        _keyboardData.PlayerPseudo = _channelName.Value;
           
        foreach (KeyboardCommand kc in _lobbyCommands)
        {
            kc.Command = _keyboardData.GetKey(kc.CommandName);
        }
        foreach (KeyboardCommand kc in _gameCommands)
        {
            kc.Command = _keyboardData.GetKey(kc.CommandName);
        }
    }

    private void Update()
    {
        if (_gameState.GetState() == GameState.State.GameListening)
        {
            SearchList(_gameCommands);
            
            if (Input.GetKeyDown(KeyCode.Space)) GameManager.Instance.ForceTurn();

            if (Input.GetKeyDown(KeyCode.T)) GameManager.Instance.ArenaTurn();
        }
        else if(_gameState.GetState() == GameState.State.LobbyListening)
        {
            SearchList(_lobbyCommands);
        }
    }

    private void SearchList(List<KeyboardCommand> keyboardCommands)
    {
        foreach (KeyboardCommand keyboardCommand in keyboardCommands)
        {
            if (!Input.GetKeyDown(keyboardCommand.Command)) continue;

            keyboardCommand.OnCommandFound.Invoke(_keyboardData.PlayerPseudo);
            break;
        }
    }
}