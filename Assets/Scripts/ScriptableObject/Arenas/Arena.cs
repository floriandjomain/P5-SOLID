using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = System.Random;

[CreateAssetMenu(menuName = "Arena/Basic")]
public class Arena : ScriptableObject
{
    protected Tile[,] Tiles;
    [SerializeField] protected Tile tilePrefab;
    protected Random rnd = new Random();

    public virtual void MapInstantiation(int playerNumber, int maxTileHealth, Action action)
    {
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
        Tile tile = Instantiate(tilePrefab, GameManager.Instance.ArenaGO.transform, true);
        
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

    public virtual void Turn()
    {
        
    }

    public void DamageTiles(List<Vector2Int> tilesToDamage, int damageAmount)
    {
        foreach (Vector2Int tile in tilesToDamage)
        {
            Tiles[tile.x,tile.y].Damage(damageAmount);
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

        for(int i = 0; i < Tiles.GetLength(0); i++)
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

    public Tile GetTilePrefab()
    {
        return tilePrefab;
    }

    public void Load(Tile[,] tileMap)
    {
        Tiles = tileMap;
    }
}
