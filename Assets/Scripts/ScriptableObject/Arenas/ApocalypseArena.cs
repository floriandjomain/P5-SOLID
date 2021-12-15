using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Arena/Apocalypse")]
public class ApocalypseArena : Arena
{
    public override void MapInstantiation(int playerNumber, int maxTileHealth, Action action)
    {
        base.MapInstantiation(playerNumber, maxTileHealth, action);

        MakeItApocalyptic(playerNumber, maxTileHealth);
    }

    private void MakeItApocalyptic(int playerNumber, int maxTileHealth)
    {
        BreakPerimeter(playerNumber);
        ApplyApocalypse(ComputeTileMap(), playerNumber, maxTileHealth);
    }

    private void BreakPerimeter(int playerNumber)
    {
        for (int i = 0; i < playerNumber; i++)
        {
            BreakTile(i % 2 == 0
                ? new Vector2Int(_rnd.Next(playerNumber), _rnd.Next(2) * (playerNumber-1))
                : new Vector2Int(_rnd.Next(2) * (playerNumber-1), _rnd.Next(playerNumber)));
        }
    }

    private void ApplyApocalypse(List<Vector2Int> tileMap, int playerNumber, int maxTileHealth)
    {
        while (tileMap.Count>playerNumber*playerNumber/2)
        {
            Vector2Int tileToBreak = tileMap[_rnd.Next(tileMap.Count)];
            
            if (!IsInArena(tileToBreak)) continue;

            int connectionCount = 0;

            Vector2Int top = new Vector2Int(tileToBreak.x, tileToBreak.y + 1);
            Vector2Int down = new Vector2Int(tileToBreak.x, tileToBreak.y - 1);
            Vector2Int right = new Vector2Int(tileToBreak.x + 1, tileToBreak.y);
            Vector2Int left = new Vector2Int(tileToBreak.x - 1, tileToBreak.y);
            
            if (IsInArena(top)) connectionCount++;
            if (IsInArena(down)) connectionCount++;
            if (IsInArena(right)) connectionCount++;
            if (IsInArena(left)) connectionCount++;
            
            if(connectionCount < 2) continue;
            
            Tiles[tileToBreak.x, tileToBreak.y].Break();
            tileMap.Remove(tileToBreak);
            
            Tiles[top.x  ,top.y  ].Damage(_rnd.Next(maxTileHealth-1)+1);
            Tiles[down.x ,down.y ].Damage(_rnd.Next(maxTileHealth-1)+1);
            Tiles[left.x ,left.y ].Damage(_rnd.Next(maxTileHealth-1)+1);
            Tiles[right.x,right.y].Damage(_rnd.Next(maxTileHealth-1)+1);
        }
    }

    private List<Vector2Int> ComputeTileMap()
    {
        List<Vector2Int> tileMap = new List<Vector2Int>();
        
        for (int i=0; i<Tiles.GetLength(0); i++)
        {
            for (int j = 0; j < Tiles.GetLength(1); j++)
            {
                if (!IsInArena(new Vector2Int(i,j))) continue;
                if (!IsInArena(new Vector2Int(i,j+1))) continue;
                if (!IsInArena(new Vector2Int(i,j-1))) continue;
                if (!IsInArena(new Vector2Int(i+1,j))) continue;
                if (!IsInArena(new Vector2Int(i-1,j))) continue;
                
                tileMap.Add(new Vector2Int(i,j));
            }
        }

        return tileMap;
    }

    public override void SetTimers()
    {
        foreach (Tile tile in Tiles)
        {
            tile.SetStartTimer(_rnd.Next(5));
        }
    }
}
