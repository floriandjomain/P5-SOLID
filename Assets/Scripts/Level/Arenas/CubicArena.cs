using System;
using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;

[CreateAssetMenu(menuName = "Arena/Cubic")]
public class CubicArena : Arena
{
    public override void MapInstantiation(int playerNumber, int maxTileHealth, Action action)
    {
        //PartieClassique 4 places/joueur
        _tiles = new Tile[playerNumber, playerNumber];
        
        for (int i = 0; i < playerNumber; i++)
        {
            for (int j = 0; j < playerNumber; j++)
            {
                Tile tile = Instantiate(tilePrefab);
                tile.SetStartLife(maxTileHealth);
                var transform = tile.transform;
                var localScale = transform.localScale;
                transform.position = new Vector3(i * localScale.x * 2.5f, 0, j * localScale.z * 2.5f);

                int distanceCenter = (int) Mathf.Sqrt(
                    Mathf.Pow(playerNumber / 2 - i, 2) +
                    Mathf.Pow(playerNumber / 2 - j, 2)
                    ) * playerNumber/2;
                
                tile.SetStartTimer(playerNumber-distanceCenter);
                tile.AddActionToDeath(action);
                _tiles[i, j] = tile;
                //Debug.Log("Tile set");
            }
        }
    }

    public override void PlacePlayers(Dictionary<string, Player> players)
    {
        //PartieClassique 4 places/joueur
        int i = 0;

        List<string> playerNames = new List<string>(players.Keys);
        foreach (string player in playerNames)
        {
            players[player].SetPos(new Vector2Int(i, i));
            //Debug.Log("placing player " + player + " at position " + players[player].GetPos());
            i ++;
        }
    }
}
