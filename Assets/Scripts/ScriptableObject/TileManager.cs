using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Manager/Tile")]
public class TileManager : ScriptableObject
{
    [SerializeField] private Tile[,] _tiles;
    [SerializeField] private Tile tilePrefab;

    public void SetUp(Dictionary<string, Player> players, int maxTileHealth)
    {
        Debug.Log("start map setup...");
        InitializeMap(players.Count, maxTileHealth);
        PlacePlayers(players);
        Debug.Log("...map setup done");
    }

    private void InitializeMap(int playerNumber, int maxTileHealth)
    {
        Debug.Log("start map initialization...");
        //PartieClassique 4 places/joueur
        _tiles = new Tile[2 * playerNumber, 2 * playerNumber];
        
        for (int i = 0; i < 2 * playerNumber; i++)
        {
            for (int j = 0; j < 2 * playerNumber; j++)
            {
                Tile tile = Instantiate(tilePrefab);
                var transform = tile.transform;
                var localScale = transform.localScale;
                transform.position = new Vector3(i * localScale.x * 2.5f, 0, j * localScale.z * 2.5f);
                _tiles[i, j] = tile;
            }
        }
        Debug.Log("...map initialization done");
    }

    private void PlacePlayers(Dictionary<string, Player> players)
    {
        Debug.Log("start placing players...");
        //PartieClassique 4 places/joueur
        int i = 1;
        
        foreach (string player in players.Keys)
        {
            players[player].SetPos(new Vector2Int(i, i-1));
            Debug.Log("placing player " + player + " at position " + players[player].GetPos());
            i += 2;
        }
        Debug.Log("...all players placed");
    }

    public Tile[,] GetTiles() => _tiles;

    public void DamageTiles(Dictionary<string, Player> players)
    {
        Vector2Int pos;

        foreach (string p in players.Keys)
        {
            if (players[p].IsAlive())
            {
                pos = players[p].GetPos();
                _tiles[pos.x,pos.y].Damage();
            }
        }
    }
}
