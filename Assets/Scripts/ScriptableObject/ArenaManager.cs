using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Manager/Tile")]
public class ArenaManager : ScriptableObject
{
    [SerializeField] private Arena Arena;
    public Action<Tile[,]> onBrokenTile;

    public void SetUp(Dictionary<string, Player> players, int maxTileHealth, Action action)
    {
        //Debug.Log("start map setup...");
        Arena.MapInstantiation(players.Count, maxTileHealth, action);
        PlacePlayers(players);
        //Debug.Log("...map setup done");
    }

    private void PlacePlayers(Dictionary<string, Player> players) => Arena.PlacePlayers(players);

    public Tile[,] GetTiles() => Arena.GetTiles();

    public void ErodeArena() => Arena.ErodeArena();

    public void DamageTiles(Dictionary<string, Player> players) => Arena.DamageTiles(players, 1);

    public void BreakTile(Vector2Int tileToBreak) => Arena.BreakTile(tileToBreak);

    public void OnBrokenTile()
    {
        
    }

    public Arena GetArena() => Arena;
}
