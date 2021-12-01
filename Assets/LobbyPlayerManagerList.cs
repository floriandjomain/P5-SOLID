using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LobbyPlayerManagerList : MonoBehaviour
{
    [SerializeField] private GameSettings gameSettings;
    [SerializeField] private PlayerManager playerManager;
    [Space(10)]
    [SerializeField] private GameObject gridLayout;
    [SerializeField] private RectTransform blackBackground;
    [SerializeField] private GameObject prefab;
    [SerializeField] private TMP_Text playerNumberText;
    [Space(10)]
    [SerializeField] private float spaceBetweenCases = 10;
    [SerializeField] private float spaceAboveBelowGrid = 50;
    [SerializeField] private int cellSizeDecrement = 50;
    [SerializeField] private float fontSizeDecrement = 2.5f;
    [SerializeField] private Vector2 cellSize = new Vector2(500, 100);
    [SerializeField] private Vector2 gridZone = new Vector2(1920, 850);

    // TO REMOVE
    public int PlayerMaxCount = 20; // use gameSettings.MaxPlayerNumber instead

    private int currentPlayerCount = 0;
    private int oldPlayerCount = 0;

    private float gridSizeY = 0;

    private List<GameObject> playersText;
    private GridLayoutGroup gridLayoutGroup;

    private void Start()
    {
        UpdateText(currentPlayerCount, PlayerMaxCount);
        playersText = new List<GameObject>();

        gridLayoutGroup = gridLayout.GetComponent<GridLayoutGroup>();
        gridLayoutGroup.cellSize = cellSize;
        
        gridSizeY = gridLayout.GetComponent<RectTransform>().sizeDelta.y;

        DisplayList(PlayerMaxCount);

        ClearNames();
    }

    // Update is called once per frame
    void Update()
    {
        currentPlayerCount = playerManager.GetCurrentPlayerNumber();
        if (currentPlayerCount != oldPlayerCount) {
            Debug.Log("Update display");
            UpdateDisplay();
            oldPlayerCount = currentPlayerCount;
        }
    }

    private void UpdateText(int currentPlayer, int maxPlayer)
    {
        playerNumberText.text = currentPlayer + " / " + maxPlayer;
    }

    private void UpdateDisplay()
    {
        ClearNames();

        int cpt = 0;
        foreach(KeyValuePair<string, Player> player in playerManager.GetPlayers())
        {
            playersText[cpt].GetComponentInChildren<TMP_Text>().text = player.Key;
            cpt++;
        }
    }

    private void ClearNames()
    {
        foreach(GameObject playerText in playersText)
        {
            playerText.GetComponentInChildren<TMP_Text>().text = "";
        }
    }

    private void DisplayList(int maxPlayer)
    {
        Vector2 _cellSize = new Vector2(cellSize.x + cellSizeDecrement, cellSize.y);
        float widthBackground = 0;
        float heightBackground = 0;
        int countDecreased = -1;

        do {
            // Calculate "best" cell size
            _cellSize.x = _cellSize.x - cellSizeDecrement;
            countDecreased += 1;

            int nbCols = Mathf.FloorToInt(gridZone.x / _cellSize.x);
            int nbRows = Mathf.CeilToInt((float)maxPlayer / nbCols);


            widthBackground = (nbCols * _cellSize.x) + (spaceBetweenCases * (nbCols + 1));
            heightBackground = (nbRows * _cellSize.y) + (spaceBetweenCases * (nbRows + 1));

        } while (heightBackground > gridSizeY - (2 * spaceAboveBelowGrid));

        // Set the size of the black background (for simulating borders)
        blackBackground.sizeDelta = new Vector2(widthBackground, heightBackground);
        gridLayoutGroup.cellSize = _cellSize;

        // Create the right number of prefabs
        for (int i = 0; i < maxPlayer; i++)
        {
            GameObject go = Instantiate(prefab, gridLayout.transform);
            TMP_Text text = go.GetComponentInChildren<TMP_Text>();
            text.fontSize = text.fontSize - (fontSizeDecrement * countDecreased);
            playersText.Add(go);
        }
    }
}
