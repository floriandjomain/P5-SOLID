using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Arena/Circle")]
public class CircleArena : Arena
{
    public override void MapInstantiation(int playerNumber, int maxTileHealth, Action action)
    {
        base.MapInstantiation(playerNumber, maxTileHealth, action);

        Vector2Int center = new Vector2Int(playerNumber / 2, playerNumber / 2);
        
        for (int i = 0; i < playerNumber; i++)
            for (int j = 0; j < playerNumber; j++)
                if (Vector2Int.Distance(new Vector2Int(i, j), center) > (int) (playerNumber / 2))
                    Tiles[i, j].Break();
    }

    public override void Turn()
    {
        Debug.Log("début d'érosion");
        foreach (Tile tile in Tiles)
        {
            if (tile.IsBroken() || _rnd.Next(3) != 0) continue;
            
            tile.TimerShot();
            //Debug.Log("erosion");
        }
        Debug.Log("fin d'érosion");
    }
}
