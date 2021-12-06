using System;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

[CreateAssetMenu(menuName = "Arena/Basic")]
public class Arena : ScriptableObject
{
    protected Tile[,] Tiles;
    [SerializeField] protected Tile tilePrefab;
    private GameObject arenaGO;
    protected Random rnd = new Random();

    private void InspectorMapInstantiation()
    {
        arenaGO = new GameObject("Arena");
    }

    public virtual void MapInstantiation(int playerNumber, int maxTileHealth, Action action)
    {
        InspectorMapInstantiation();
        
        //Arene classique
        Tiles = new Tile[playerNumber, playerNumber];
        
        for (int i = 0; i < playerNumber; i++)
        {
            for (int j = 0; j < playerNumber; j++)
            {
                Tile tile = CreateTile(maxTileHealth, action);
                
                Transform transform = tile.transform;
                Vector3 localScale = transform.localScale;
                transform.position = new Vector3(i * localScale.x * 2.5f, 0, j * localScale.z * 2.5f);
                tile.name = "tile("+i+","+j+")";
                
                Tiles[i, j] = tile;
                //Debug.Log("Tile set");
            }
        }
    }

    private Tile CreateTile(int maxTileHealth, Action action)
    {
        Tile tile = Instantiate(tilePrefab, arenaGO.transform, true);
        
        tile.SetStartLife(maxTileHealth);
        tile.AddActionToDeath(action);
        
        return tile;
    }
    
    public bool IsInArena(Vector2Int pos)
    {
        if (pos.x < 0 || pos.x >= Tiles.GetLength(0) || pos.y < 0 || pos.y >= Tiles.GetLength(1))
            return false;

        return !Tiles[pos.x, pos.y].IsBroken();
    }

    public Tile[,] GetTiles() => Tiles;

    public void ErodeArena()
    {
        Debug.Log("début d'érosion");
        foreach (Tile tile in Tiles)
        {
            if (tile.IsBroken() || rnd.Next(3) != 0) continue;
            
            tile.TimerShot();
            Debug.Log("erosion");
        }
        Debug.Log("fin d'érosion");
    }

    public void DamageTiles(Dictionary<string, Player> players, int damageAmount)
    {
        Vector2Int pos;

        List<string> playerNames = new List<string>(players.Keys);
        foreach (string p in playerNames)
        {
            if (!players[p].IsAlive()) continue;
            
            pos = players[p].GetPos();
            Tiles[pos.x,pos.y].Damage(damageAmount);
        }
    }

    public void BreakTile(Vector2Int tileToBreak)
    {
        if(IsInArena(tileToBreak))
            Tiles[tileToBreak.x, tileToBreak.y].Break();
    }

    public List<Vector2Int> GetWalkableTilesPositions()
    {
        List<Vector2Int> walkableTilesPositions = new List<Vector2Int>();

        for(int i=0; i<Tiles.GetLength(0); i++)
            for (int j = 0; j < Tiles.GetLength(1); j++)
                if (!Tiles[i,j].IsBroken()) walkableTilesPositions.Add(new Vector2Int(i,j));

        return walkableTilesPositions;
    }

    public virtual void SetTimers()
    {
        int playerNumber = Tiles.GetLength(0);

        for (int i = 0; i < playerNumber; i++)
        {
            for (int j = 0; j < playerNumber; j++)
            {
                int distanceCenter = (int) Mathf.Sqrt(
                    Mathf.Pow(playerNumber / 2 - i, 2) +
                    Mathf.Pow(playerNumber / 2 - j, 2)
                ) * playerNumber / 2;

                Tiles[i, j].SetStartTimer(playerNumber - distanceCenter);
            }
        }
    }
}
