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
    [SerializeField] private int cellSizeDecrement = 50;
    [SerializeField] private float fontSizeDecrement = 2.5f;
    [SerializeField] private Vector2 cellSize = new Vector2(500, 100);

    private int currentPlayerCount = 0;
    private int oldPlayerCount = 0;

    private Vector2 gridSize = Vector2.zero;

    private List<GameObject> playersText;
    private GridLayoutGroup gridLayoutGroup;

    public void CreateLobby()
    {
        playersText = new List<GameObject>();
        gridLayoutGroup = gridLayout.GetComponent<GridLayoutGroup>();
        gridLayoutGroup.cellSize = cellSize;
        gridSize = gridLayout.GetComponent<RectTransform>().sizeDelta;

        UpdateText(currentPlayerCount, gameSettings.MaxPlayerNumber);   
        DisplayList(gameSettings.MaxPlayerNumber);
        SetAllNames();
    }

    // Update is called once per frame
    void Update()
    {
        currentPlayerCount = playerManager.GetCurrentPlayerNumber();
        if (currentPlayerCount != oldPlayerCount) {
            UpdateDisplay();
            UpdateText(currentPlayerCount, gameSettings.MaxPlayerNumber);
            oldPlayerCount = currentPlayerCount;
        }
    }

    private void UpdateText(int currentPlayer, int maxPlayer)
    {
        playerNumberText.text = currentPlayer + " / " + maxPlayer;
    }

    private void UpdateDisplay()
    {
        SetAllNames();

        int cpt = 0;
        foreach(KeyValuePair<string, Player> player in playerManager.GetPlayers())
        {
            playersText[cpt].GetComponentInChildren<TMP_Text>().text = player.Key;
            cpt++;
        }
    }

    private void ClearDisplay()
    {
        foreach(Transform child in gridLayout.transform)
        {
            Destroy(child.gameObject);
            playersText.Clear();
        }
    }

    private void ClearPlayers()
    {
        playerManager.RemoveAllPlayers();
    }

    public void ClearData()
    {
        ClearDisplay();
        ClearPlayers();
    }

    private void SetAllNames()
    {
        foreach(GameObject playerText in playersText)
        {
            playerText.GetComponentInChildren<TMP_Text>().text = "";
        }
    }

    private void DisplayList(int maxPlayer)
    {
        Vector2 __cellSize = new Vector2(cellSize.x + cellSizeDecrement, cellSize.y);
        float widthBackground = 0;
        float heightBackground = 0;
        int countDecreased = -1;

        do {
            // Calculate "best" cell size
            __cellSize.x = __cellSize.x - cellSizeDecrement;
            countDecreased += 1;

            int nbCols = -1;
            int nbRows = 1;

            float calcuation = (maxPlayer * __cellSize.x) + (spaceBetweenCases * (maxPlayer + 1));
            if (calcuation > gridSize.x)
            {
                // BUG : if > 35 , calculation become wrong
                nbCols = Mathf.FloorToInt(gridSize.x / __cellSize.x);
                nbRows = Mathf.CeilToInt((float)maxPlayer / nbCols);

                widthBackground = (nbCols * __cellSize.x) + (spaceBetweenCases * (nbCols + 1));
            }
            else
            {
                widthBackground = calcuation;
            }
           
            heightBackground = (nbRows * __cellSize.y) + (spaceBetweenCases * (nbRows + 1));

        } while (heightBackground >= gridSize.y);

        // Set the size of the black background (for simulating borders)
        blackBackground.sizeDelta = new Vector2(widthBackground, heightBackground);
        gridLayoutGroup.cellSize = __cellSize;

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
