using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardListener : MonoBehaviour
{
    [SerializeField] private GameState _gameState;
    [SerializeField] private GameSettings _gameSettings;
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private GameObject _commands;
    [SerializeField] private KeyboardData[] _keyboardDataList;
    private KeyboardData _keyboardData;

    private List<KeyboardCommand> _lobbyKeyboardCommands;
    private List<KeyboardCommand> _gameKeyboardCommands;

    private void Start()
    {
        _lobbyKeyboardCommands = new List<KeyboardCommand>();
        _gameKeyboardCommands = new List<KeyboardCommand>();

        foreach (KeyboardData keyboardData in _keyboardDataList)
            if (_gameSettings.StreamerControlType == keyboardData.DataName)
                _keyboardData = keyboardData;

        if (!_keyboardData) _keyboardData = _keyboardDataList[0];
        
        //keyboardData.PlayerPseudo = channelName.Value;
            
        if (_commands != null)
        {
            KeyboardCommand[] commands = _commands.GetComponents<KeyboardCommand>();
            foreach (KeyboardCommand kc in commands)
            {
                kc.Command = _keyboardData.GetKey(kc.CommandName);
                //Debug.Log($"key registered {kc.CommandName}");
                
                if(kc.commandType == KeyboardCommand.CommandType.Game)
                {
                    _gameKeyboardCommands.Add(kc);
                }
                else if (kc.commandType == KeyboardCommand.CommandType.Lobby)
                {
                    _lobbyKeyboardCommands.Add(kc);
                }
            }
        }
        else
        {
            Debug.Log("[KeyboardInterpreter] There is no commands");
        }
    }

    private void Update()
    {
        if (_gameState.GetState() == GameState.State.GameListening)
        {
            foreach (KeyboardCommand keyboardCommand in _gameKeyboardCommands)
            {
                if (!Input.GetKeyDown(keyboardCommand.Command)) continue;
                
                //Debug.Log("[Keyboard Listener] on passe combien de fois ici ?");
                
                keyboardCommand.onCommandFound.Invoke(_keyboardData.PlayerPseudo);
                break;
            }
            
            if (Input.GetKeyDown(KeyCode.Space)) _gameManager.ForceTurn();

            if (Input.GetKeyDown(KeyCode.T)) _gameManager.ArenaTurn();
        }
        else if(_gameState.GetState() == GameState.State.LobbyListening)
        {
            foreach (KeyboardCommand keyboardCommand in _lobbyKeyboardCommands)
            {
                if (!Input.GetKeyDown(keyboardCommand.Command)) continue;

                keyboardCommand.onCommandFound.Invoke(_keyboardData.PlayerPseudo);
                break;
            }
        }
    }
}