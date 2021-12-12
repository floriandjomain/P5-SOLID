using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardListener : MonoBehaviour
{
    [SerializeField] private GameState _gameState;
    [SerializeField] private GameSettings _gameSettings;
    // [SerializeField] private GameManager _gameManager;
    [SerializeField] private List<KeyboardCommand> _lobbyKeyboardCommands;
    [SerializeField] private List<KeyboardCommand> _gameKeyboardCommands;

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
        // Key board name in case joins
        _keyboardData.PlayerPseudo = _channelName.Value;
           
        foreach (KeyboardCommand kc in _lobbyKeyboardCommands)
        {
            kc.Command = _keyboardData.GetKey(kc.CommandName);
        }
        foreach (KeyboardCommand kc in _gameKeyboardCommands)
        {
            kc.Command = _keyboardData.GetKey(kc.CommandName);
        }
    }

    private void Update()
    {
        if (_gameState.GetState() == GameState.State.GameListening)
        {
            foreach (KeyboardCommand keyboardCommand in _gameKeyboardCommands)
            {
                if (!Input.GetKeyDown(keyboardCommand.Command)) continue;
                                
                keyboardCommand.OnCommandFound.Invoke(_keyboardData.PlayerPseudo);
                break;
            }
            
            if (Input.GetKeyDown(KeyCode.Space)) GameManager.Instance.ForceTurn();

            if (Input.GetKeyDown(KeyCode.T)) GameManager.Instance.ArenaTurn();
        }
        else if(_gameState.GetState() == GameState.State.LobbyListening)
        {
            foreach (KeyboardCommand keyboardCommand in _lobbyKeyboardCommands)
            {
                if (!Input.GetKeyDown(keyboardCommand.Command)) continue;

                keyboardCommand.OnCommandFound.Invoke(_keyboardData.PlayerPseudo);
                break;
            }
        }
    }
}