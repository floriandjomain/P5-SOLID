using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILobbyManager : MonoBehaviour
{
    [SerializeField] private PlayerManager playerManager;
    [SerializeField] private Button _launchPlayerButton;
    [SerializeField] private int _minNumberPlayer;


    private void Update()
    {
        if(playerManager.GetCurrentPlayerNumber() < _minNumberPlayer)
        {
            _launchPlayerButton.interactable = false;
        }
        else
        {
            _launchPlayerButton.interactable = true;
        }
    }
}
