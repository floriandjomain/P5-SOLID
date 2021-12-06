using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public abstract class Arena : ScriptableObject
{
    [SerializeField] protected Tile[,] _tiles;
    [SerializeField] protected Tile tilePrefab;
    protected GameObject arenaGO;
    private Random rnd = new Random();
    public event Action onDestroyedTile;

    protected void MapInstantiation()
    {
        arenaGO = new GameObject("Arena");
    }

    public abstract void MapInstantiation(int playerNumber, int maxTileHealth, Action action);
    
    public bool IsInArena(Vector2Int pos)
    {
        if (pos.x < 0 || pos.x >= _tiles.GetLength(0) || pos.y < 0 || pos.y >= _tiles.GetLength(1))
            return false;

        return !_tiles[pos.x, pos.y].IsBroken();
    }

    public Tile[,] GetTiles() => _tiles;

    public void ErodeArena()
    {
        Debug.Log("début d'érosion");
        foreach (Tile tile in _tiles)
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
            _tiles[pos.x,pos.y].Damage(damageAmount);
        }
    }

    public void BreakTile(Vector2Int tileToBreak)
    {
        if(IsInArena(tileToBreak))
            _tiles[tileToBreak.x, tileToBreak.y].Break();
    }

    public abstract void PlacePlayers(Dictionary<string, Player> players);

    public void OnDestroyedTile()
    {
        onDestroyedTile?.Invoke();
    }
}
