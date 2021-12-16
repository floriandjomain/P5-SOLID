using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Manager/Tile")]
public class ArenaManager : ScriptableObject
{
    [SerializeField] private Arena _arena;
    private Action<Tile[,]> _onBrokenTile;

    public Action<Tile[,]> OnBrokenTile { set => _onBrokenTile = value; }
    public Arena Arena { get => _arena; set => _arena = value; }

    public void SetUp(int playerCount, int maxTileHealth, Action action)
    {
        //Debug.Log("[ArenaManager] start map setup...");
        _arena.MapInstantiation(playerCount, maxTileHealth, action);
        _arena.SetTimers();
        //Debug.Log("[ArenaManager] ...map setup done");
    }

    public List<Vector2Int> GetWalkableTilesPositions() => _arena.GetWalkableTilesPositions();

    public Tile[,] GetTiles() => _arena.Tiles;

    public void Turn() => _arena.Turn();

    public void DamageTiles(List<Vector2Int> tilesToDamage, int damageAmount = 1)
    {
        _arena.DamageTiles(tilesToDamage, damageAmount);
    }

    public void BreakTile(Vector2Int tileToBreak) => _arena.BreakTile(tileToBreak);
    
    public Tile GetTilePrefab()
    {
        return _arena.TilePrefab;
    }

    public void Load(Arena arena)
    {
        _arena = arena;
    }

    public int GetMapSize()
    {
        return Arena.MapSize;
    }
}
