using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyManager : MonoBehaviour
{
    [SerializeField] private PlayerManager _playerManager;

    private static LobbyManager _instance;

    public static LobbyManager Instance { get => _instance; }

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(this);
            return;
        }
        _instance = this;
    }

    public void AddPlayer(string player)
    {
        _playerManager.AddPlayer(player);
    }

    public void RemovePlayer(string player)
    {
        _playerManager.RemovePlayer(player);
    }
}
