using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Manager/Tile")]
public class ArenaManager : ScriptableObject
{
    [SerializeField] private Arena Arena;
    public Action<Tile[,]> onBrokenTile;

    public void SetUp(int playerCount, int maxTileHealth, Action action)
    {
        //Debug.Log("[ArenaManager] start map setup...");
        Arena.MapInstantiation(playerCount, maxTileHealth, action);
        Arena.SetTimers();
        //Debug.Log("[ArenaManager] ...map setup done");
    }

    public List<Vector2Int> GetWalkableTilesPositions() => Arena.GetWalkableTilesPositions();

    public Tile[,] GetTiles() => Arena.GetTiles();

    public void Turn() => Arena.Turn();

    public void DamageTiles(List<Vector2Int> tilesToDamage, int damageAmount = 1)
    {
        Arena.DamageTiles(tilesToDamage, damageAmount);
    }

    public void BreakTile(Vector2Int tileToBreak) => Arena.BreakTile(tileToBreak);

    public Arena GetArena() => Arena;

    public Arena SetArena(Arena arena) => Arena = arena;
}
