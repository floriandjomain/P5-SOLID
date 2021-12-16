using System;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public abstract class Arena : ScriptableObject
{
    protected Tile[,] _tiles;
    public Tile[,] Tiles { get => _tiles; }

    [SerializeField] protected Tile _tilePrefab;
    public Tile TilePrefab { get => _tilePrefab; }
    [SerializeField] protected int _minimumSizeOfMap;
    protected int _mapSize;
    public int MapSize { get => _mapSize; }

    public virtual void MapInstantiation(int playerNumber, int maxTileHealth, Action action)
    {
        CheckMapSize(playerNumber);
        
        //Arene classique
        _tiles = new Tile[_mapSize, _mapSize];
        
        for (int i = 0; i < _mapSize; i++)
        {
            for (int j = 0; j < _mapSize; j++)
            {
                Tile tile = CreateTile(maxTileHealth, action);
                
                Transform transform = tile.transform;
                Vector3 localScale = transform.localScale;
                transform.position = new Vector3(i * localScale.x * 2.5f, 0, j * localScale.z * 2.5f);
                tile.name = "tile("+i+","+j+")";
                
                _tiles[i, j] = tile;
                // Debug.Log("Tile set");
            }
        }
    }

    private void CheckMapSize(int playerNumber)
    {
        _mapSize = playerNumber < _minimumSizeOfMap ? _minimumSizeOfMap : playerNumber;
    }

    private Tile CreateTile(int maxTileHealth, Action action)
    {
        Tile tile = Instantiate(_tilePrefab, GameManager.Instance.ArenaGO.transform, true);
        tile.gameObject.SetActive(true);
        
        tile.SetStartLife(maxTileHealth);
        tile.AddActionToDeath(action);

        return tile;
    }
    
    public bool IsInArena(Vector2Int pos)
    {
        if (pos.x < 0 || pos.x >= _tiles.GetLength(0) || pos.y < 0 || pos.y >= _tiles.GetLength(1))
            return false;

        return !_tiles[pos.x, pos.y].IsBroken();
    }
    
    public abstract void Turn();

    public void DamageTiles(List<Vector2Int> tilesToDamage, int damageAmount)
    {
        foreach (Vector2Int tile in tilesToDamage)
        {
            _tiles[tile.x,tile.y].Damage(damageAmount);
        }
    }

    public void BreakTile(Vector2Int tileToBreak)
    {
        if(IsInArena(tileToBreak))
            _tiles[tileToBreak.x, tileToBreak.y].Break();
    }

    public void BreakTile(int i, int j)
    {
        BreakTile(new Vector2Int(i,j));
    }

    public List<Vector2Int> GetWalkableTilesPositions()
    {
        List<Vector2Int> walkableTilesPositions = new List<Vector2Int>();

        for(int i = 0; i < _tiles.GetLength(0); i++)
            for (int j = 0; j < _tiles.GetLength(1); j++)
                if (!_tiles[i,j].IsBroken()) walkableTilesPositions.Add(new Vector2Int(i,j));

        return walkableTilesPositions;
    }

    public virtual void SetTimers()
    {
        int playerNumber = _tiles.GetLength(0);

        for (int i = 0; i < playerNumber; i++)
        {
            for (int j = 0; j < playerNumber; j++)
            {
                int distanceCenter = (int) Mathf.Sqrt(
                    Mathf.Pow(playerNumber / 2 - i, 2) +
                    Mathf.Pow(playerNumber / 2 - j, 2)
                ) * playerNumber / 2;

                _tiles[i, j].SetStartTimer(playerNumber - distanceCenter);
            }
        }
    }

    public void Load(Tile[,] tileMap)
    {
        _tiles = tileMap;
    }
}
