using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Arena/Circle")]
public class CircleArena : Arena
{
    public override void MapInstantiation(int playerNumber, int maxTileHealth, Action action)
    {
        base.MapInstantiation(playerNumber, maxTileHealth, action);

        Vector2Int center = new Vector2Int(_mapSize / 2, _mapSize / 2);
        
        for (int i = 0; i < _mapSize; i++)
            for (int j = 0; j < _mapSize; j++)
                if (Vector2Int.Distance(new Vector2Int(i, j), center) > (int) (_mapSize / 2))
                    Tiles[i, j].Break();

        if (_mapSize % 2 == 0)
        {
            BreakTile(_mapSize/2-1, 0);
            BreakTile(0, _mapSize/2-1);
        }
    }

    public override void Turn()
    {
        Debug.Log("début d'érosion");
        foreach (Tile tile in Tiles)
        {
            if (tile.IsBroken() || GameManager.Instance.Rnd.Next(3) != 0) continue;
            
            tile.TimerShot();
            //Debug.Log("erosion");
        }
        Debug.Log("fin d'érosion");
    }
}
