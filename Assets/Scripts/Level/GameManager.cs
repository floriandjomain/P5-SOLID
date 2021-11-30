using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TileManager _tileManager;
    [SerializeField] private PlayerManager _playerManager;
    [SerializeField] private GameSettings _gameSettings;

    [SerializeField] private string[] cheatCode;

    private void Awake()
    {
        SetUp();
    }

    private void Cheat()
    {
        Debug.Log("!!!cheat code activated!!!");
        foreach (string playerName in cheatCode) _playerManager.AddPlayer(playerName);
        Debug.Log("!!!cheat code used!!!");
    }

    public void SetUp()
    {
        Cheat();
        
        Debug.Log("start game setup...");
        _playerManager.SetUp();
        _tileManager.SetUp(_playerManager.GetPlayers(), _gameSettings.TileMaxLifePoints);
        Debug.Log("...game setup done");
    }
    
    public void PlayTurn()
    {
        _tileManager.DamageTiles(_playerManager.GetPlayers());
        _playerManager.Turn(_tileManager.GetTiles());
    }

    public void SetMovement(string player, Movement move)
    {
        _playerManager.SetMovement(player, move);
    }

    public bool AddPlayer(string playerPseudo)
    {
        if (_playerManager.GetCurrentPlayerNumber() >= _gameSettings.MaxPlayerNumber) return false;
            
        _playerManager.AddPlayer(playerPseudo);
        return true;
    }

    public void RemovePlayer(string playerPseudo) => _playerManager.RemovePlayer(playerPseudo);
}
