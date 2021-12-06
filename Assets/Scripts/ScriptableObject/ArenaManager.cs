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
        //Debug.Log("start map setup...");
        Arena.MapInstantiation(playerCount, maxTileHealth, action);
        Arena.SetTimers();
        //Debug.Log("...map setup done");
    }

    public List<Vector2Int> GetWalkableTilesPositions() => Arena.GetWalkableTilesPositions();

    public Tile[,] GetTiles() => Arena.GetTiles();

    public void ErodeArena() => Arena.ErodeArena();

    public void DamageTiles(Dictionary<string, Player> players) => Arena.DamageTiles(players, 1);

    public void BreakTile(Vector2Int tileToBreak) => Arena.BreakTile(tileToBreak);

    public Arena GetArena() => Arena;
}
