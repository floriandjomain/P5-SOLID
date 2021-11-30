using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Manager/Tile")]
public class TileManager : ScriptableObject
{
    [SerializeField] private Tile[][] Tiles;

    public void SetSize()
    {
            
    }

    public Tile[][] GetTiles() => Tiles;

    public void DamageTiles(Dictionary<string, Player> players)
    {
        Vector2Int pos;

        foreach (string p in players.Keys)
        {
            if (players[p].IsAlive())
            {
                pos = players[p].GetPos();
                Tiles[pos.x][pos.y].Damage();
            }
        }
    }
}
