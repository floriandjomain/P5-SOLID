using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILobbyManager : MonoBehaviour
{
    [SerializeField] private PlayerManager _playerManager;
    [SerializeField] private Button _launchPlayerButton;
    [SerializeField] private IntVariable _minNumberPlayer;

    private void Update()
    {
        if (_playerManager.GetCurrentPlayerNumber() < _minNumberPlayer.Value)
        {
            _launchPlayerButton.interactable = false;
        }
        else
        {
            _launchPlayerButton.interactable = true;
        }
    }
}
