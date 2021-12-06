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
                if (Vector2Int.Distance(new Vector2Int(i, j), center) > playerNumber / 2)
                    Tiles[i, j].Break();
    }
}
