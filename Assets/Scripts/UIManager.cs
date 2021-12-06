using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameState _gameState;
    [Space(10)]
    [SerializeField] private GameObject _allMenu;
    [Space(10)]
    [SerializeField] private GameObject _mainMenu;
    [SerializeField] private GameObject _settingsMenu;
    [SerializeField] private GameObject _lobbyMenu;
    [Space(10)]
    [SerializeField] private GameObject _gameGUI;


    private void Start()
    {
        _gameState.SetState(GameState.State.NotListening);

        _allMenu.SetActive(true);

        _mainMenu.SetActive(true);
        _settingsMenu.SetActive(false);
        _lobbyMenu.SetActive(false);

        _gameGUI.SetActive(false);

        GoToLobbyMenu();
    }

    public void GoToSettingsMenu()
    {
        _mainMenu.SetActive(false);
        _settingsMenu.SetActive(true);
        _lobbyMenu.SetActive(false);
    }

    public void GoToLobbyMenu()
    {
        _mainMenu.SetActive(false);
        _settingsMenu.SetActive(false);
        _lobbyMenu.SetActive(true);

        _gameState.SetState(GameState.State.LobbyListening);

        ScenesManager.Instance.StartTwitchBot();
    }


    public void GoToGameView()
    {
        _allMenu.SetActive(false);

        _mainMenu.SetActive(false);
        _settingsMenu.SetActive(false);
        _lobbyMenu.SetActive(false);

        _gameGUI.SetActive(true);

        StartCoroutine(ScenesManager.Instance.StartGame());
    }


    public void QuitGame()
    {
        Application.Quit();
    }
}
