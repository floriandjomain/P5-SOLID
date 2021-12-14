using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LobbyPlayerManagerList : MonoBehaviour
{
    [SerializeField] private GameSettings _gameSettings;
    [SerializeField] private PlayerManager _playerManager;
    [Space(10)]
    [SerializeField] private GameObject _gridLayout;
    [SerializeField] private RectTransform _blackBackground;
    [SerializeField] private GameObject _prefab;
    [SerializeField] private TMP_Text _playerNumberText;
    [Space(10)]
    [SerializeField] private float _spaceBetweenCases = 10;
    [SerializeField] private int _cellSizeDecrement = 50;
    [SerializeField] private float _fontSizeDecrement = 2.5f;
    [SerializeField] private Vector2 _cellSize = new Vector2(500, 100);

    private int _currentPlayerCount = 0;
    private int _oldPlayerCount = 0;

    private Vector2 _gridSize = Vector2.zero;

    private List<GameObject> _playersText;
    private GridLayoutGroup _gridLayoutGroup;

    public void CreateLobby()
    {
        _playersText = new List<GameObject>();
        _gridLayoutGroup = _gridLayout.GetComponent<GridLayoutGroup>();
        _gridLayoutGroup.cellSize = _cellSize;
        _gridSize = _gridLayout.GetComponent<RectTransform>().sizeDelta;

        UpdateText(_currentPlayerCount, _gameSettings.MaxPlayerNumber);   
        DisplayList(_gameSettings.MaxPlayerNumber);
        SetAllNames();
    }

    // Update is called once per frame
    void Update()
    {
        _currentPlayerCount = _playerManager.GetCurrentPlayerNumber();
        if (_currentPlayerCount != _oldPlayerCount) {
            UpdateDisplay();
            UpdateText(_currentPlayerCount, _gameSettings.MaxPlayerNumber);
            _oldPlayerCount = _currentPlayerCount;
        }
    }

    private void UpdateText(int currentPlayer, int maxPlayer)
    {
        _playerNumberText.text = currentPlayer + " / " + maxPlayer;
    }

    private void UpdateDisplay()
    {
        SetAllNames();

        int cpt = 0;
        foreach(KeyValuePair<string, Player> player in _playerManager.GetPlayers())
        {
            _playersText[cpt].GetComponentInChildren<TMP_Text>().text = player.Key;
            cpt++;
        }
    }

    private void ClearDisplay()
    {
        foreach(Transform child in _gridLayout.transform)
        {
            Destroy(child.gameObject);
            _playersText.Clear();
        }
    }

    public void ClearPlayers()
    {
        _playerManager.RemoveAllPlayers();
    }

    public void RemoveMovements()
    {
        _playerManager.RemoveAllMovementPlayer();
    }

    public void ClearData()
    {
        ClearDisplay();
        ClearPlayers();
    }

    public void SetAllNames()
    {
        foreach(GameObject playerText in _playersText)
        {
            playerText.GetComponentInChildren<TMP_Text>().text = "";
        }
    }

    private void DisplayList(int maxPlayer)
    {
        Vector2 cellSize = new Vector2(_cellSize.x + _cellSizeDecrement, _cellSize.y);
        float widthBackground = 0;
        float heightBackground = 0;
        int countDecreased = -1;

        do {
            // Calculate "best" cell size
            cellSize.x = cellSize.x - _cellSizeDecrement;
            countDecreased += 1;

            int nbCols = -1;
            int nbRows = 1;

            float calcuation = (maxPlayer * cellSize.x) + (_spaceBetweenCases * (maxPlayer + 1));
            if (calcuation > _gridSize.x)
            {
                nbCols = Mathf.FloorToInt(_gridSize.x / (cellSize.x + _spaceBetweenCases));
                nbRows = Mathf.CeilToInt((float)maxPlayer / nbCols);

                widthBackground = (nbCols * cellSize.x) + (_spaceBetweenCases * (nbCols + 1));
            }
            else
            {
                widthBackground = calcuation;
            }
           
            heightBackground = (nbRows * cellSize.y) + (_spaceBetweenCases * (nbRows + 1));

        } while (heightBackground >= _gridSize.y);

        // Set the size of the black background (for simulating borders)
        _blackBackground.sizeDelta = new Vector2(widthBackground, heightBackground);
        _gridLayoutGroup.cellSize = cellSize;

        // Create the right number of prefabs
        for (int i = 0; i < maxPlayer; i++)
        {
            GameObject go = Instantiate(_prefab, _gridLayout.transform);
            TMP_Text text = go.GetComponentInChildren<TMP_Text>();
            text.fontSize = text.fontSize - (_fontSizeDecrement * countDecreased);
            _playersText.Add(go);
        }
    }
}
