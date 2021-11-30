using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyManager : MonoBehaviour
{
    [SerializeField] private DictionaryPlayer dictionaryPlayer;

    private static LobbyManager _instance;

    public static LobbyManager Instance
    {
        get => _instance;
    }

    private void Awake()
    {
        if (_instance != null && _instance != this) Destroy(gameObject);
        _instance = this;
    }

    public void AddPlayer(string player)
    {
        Debug.Log("[LobbyManager] Add Player");
        dictionaryPlayer.AddPlayer(player);
    }

    public void RemovePlayer(string player)
    {
        Debug.Log("[LobbyManager] Remove Player");
        dictionaryPlayer.RemovePlayer(player);
    }
}
