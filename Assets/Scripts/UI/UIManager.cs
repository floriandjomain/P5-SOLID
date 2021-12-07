using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] private GameState _gameState;
    [Space(10)]
    [SerializeField] private GameObject _allMenu;
    [Space(10)]
    [SerializeField] private GameObject _mainMenu;
    [SerializeField] private GameObject _settingsMenu;
    [SerializeField] private GameObject _lobbyMenu;
    [Space(10)]
    [SerializeField] private GameObject _gameGUI;
    // [SerializeField] private 
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }
        Instance = this;

    }

    private void Start()
    {
        _gameState.SetState(GameState.State.NotListening);

        _allMenu.SetActive(true);

        _mainMenu.SetActive(true);
        _settingsMenu.SetActive(false);
        _lobbyMenu.SetActive(false);

        _gameGUI.SetActive(false);

        // GoToGameView();

        /// Observer qui va s'execute à la fin des coroutines de transition
        CoroutineManager.instance.OnEndUICoroutine += SwitchMenu;
    }

    public void SwitchMenu(string wichMenu)
    {
        if(wichMenu == "GoToSettingsMenu") { GoToSettingsMenu(); }
        else if (wichMenu == "GoToLobbyMenu") { GoToLobbyMenu(); }
        else if (wichMenu == "GoToGameView") { GoToGameView(); }
    }

    public void GoToSettingsMenu()
    {
        EnableMenu();
        _mainMenu.SetActive(false);
        _settingsMenu.SetActive(true);
        _lobbyMenu.SetActive(false);
    }

    public void GoToLobbyMenu()
    {
        EnableMenu();
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

    public void EnableMenu()
    {
        _allMenu.SetActive(true);
    }

    public void DisableMenu()
    {
        _allMenu.SetActive(false);
    }


    public void QuitGame()
    {
        Application.Quit();
    }
}
