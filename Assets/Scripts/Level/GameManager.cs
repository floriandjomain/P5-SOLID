using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TileManager _tileManager;
    [SerializeField] private PlayerManager _playerManager;
    [SerializeField] private GameSettings _gameSettings;
        
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
