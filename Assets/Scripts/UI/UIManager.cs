using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance;
    public static UIManager Instance { get => _instance; }

    [SerializeField] private GameState _gameState;
    [SerializeField] private GameSettings _gameSettings;
    [Space(10)]
    [SerializeField] private GameObject _allMenu;
    [Space(10)]
    [SerializeField] private GameObject _mainMenu;
    [SerializeField] private GameObject _settingsMenu;
    [SerializeField] private GameObject _lobbyMenu;
    [Space(10)]
    [SerializeField] private GameObject _gameGUI;
    [Space(10)]
    [SerializeField] private GameObject _onGame;
    [SerializeField] private GameObject _endGame;
    [Space(10)]
    [SerializeField] private LobbyPlayerManagerList _lobbyPlayerManagerList;

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(this);
            return;
        }
        _instance = this;
    }

    private void Start()
    {
        _gameState.SetState(GameState.State.NotListening);

        GoToMainMenu();

        /// Observer qui va s'execute à la fin des coroutines de transition
        CoroutineManager.instance.OnEndUICoroutine += SwitchMenu;
    }

    public void SwitchMenu(string wichMenu)
    {
        if(wichMenu == "GoToSettingsMenu") { GoToSettingsMenu(); }
        else if (wichMenu == "GoToLobbyMenu") { GoToLobbyMenu(); }
        else if (wichMenu == "GoToGameView") { GoToGameView(); }
    }

    public void GoToMainMenu()
    {
        EnableMenu();

        _mainMenu.SetActive(true);
        _settingsMenu.SetActive(false);
        _lobbyMenu.SetActive(false);

        _gameGUI.SetActive(false);
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

        _gameGUI.SetActive(false);

        _gameState.SetState(GameState.State.LobbyListening);
        ScenesManager.Instance.UnloadLevelIfLoaded();

        if (_gameSettings.KeepPlayersAfterFinish)
        {
            Debug.Log("KeepPlayersAfterFinish");
            _lobbyPlayerManagerList.RemoveMovements();
        }
        else
        {
            Debug.Log("! KeepPlayersAfterFinish");

            _lobbyPlayerManagerList.ClearPlayers();
            _lobbyPlayerManagerList.SetAllNames();
        }
    }

    public void GoToLobbyMenuWithTwitchBot()
    {
        _lobbyPlayerManagerList.CreateLobby();
        ScenesManager.Instance.StartTwitchBot();

        GoToLobbyMenu();
    }

    public void DisableTwitchBot()
    {
        ScenesManager.Instance.StopTwitchBot();
    }


    public void GoToGameView()
    {
        _allMenu.SetActive(false);

        _mainMenu.SetActive(false);
        _settingsMenu.SetActive(false);
        _lobbyMenu.SetActive(false);

        _gameGUI.SetActive(true);

        _onGame.SetActive(true);
        _endGame.SetActive(false);
    
        StartCoroutine(ScenesManager.Instance.StartGame());
    }

    public void GoToEndGameView()
    {
        _onGame.SetActive(false);
        _endGame.SetActive(true);
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

    public void DisplayEndText(List<string> winners)
    {
        GoToEndGameView();

        string winnerText = "";
        foreach (string winner in winners)
        {
            winnerText += winner + "\n";
        }
        winnerText += "won !";

        _endGame.GetComponentInChildren<TMP_Text>().text = winnerText;
    }
}
