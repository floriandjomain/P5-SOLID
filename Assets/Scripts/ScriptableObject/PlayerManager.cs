using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Manager/Player")]
public class PlayerManager : ScriptableObject
{
    [SerializeField] private Dictionary<string, Player> Players = new Dictionary<string, Player>();
    [SerializeField] private Dictionary<string, Movement> Movements = new Dictionary<string, Movement>();

    [SerializeField] private Player playerPrefab;

    public Dictionary<string, Player> GetPlayers() => Players;
    public int GetCurrentPlayerNumber() => Players.Count;

    public void Turn(Tile[,] tiles)
    {
        CompileMovements(tiles);
        ApplyMovements();
        Falls(tiles);
        ResetMovements();
    }

    private void CompileMovements(Tile[,] tiles)
    {
        foreach (string p in Players.Keys)
        {
            CompileMovement(Players[p], Movements[p], tiles);
        }
    }

    private static void CompileMovement(Player player, Movement movement, Tile[,] tiles)
    {
        Vector2Int pos = player.GetPos() + MovementManager.GetVector(movement);

        if (pos.x < 0 || pos.y < 0 || pos.x >= tiles.Length || pos.y >= tiles.Length) return;

        if (!tiles[pos.x, pos.y].isActiveAndEnabled) return;

        player.SetNextPos(pos);
    }

    private void ApplyMovements()
    {
        List<string> conflictedPlayers = CheckForConflicts();
        
        foreach (string p in Players.Keys)
        {
            if (Players[p].IsAlive() && !conflictedPlayers.Contains(p))
                Players[p].ApplyMovement();
        }
    }

    private List<string> CheckForConflicts()
    {
        List<string> conflictedPlayers = new List<string>();
        int conflictedPlayersNumber = 0;
        Vector2Int p1Pos, p2Pos;

        do
        {
            foreach (string p1 in Players.Keys)
            {
                if(!Players[p1].IsAlive() || conflictedPlayers.Contains(p1)) continue;
                
                p1Pos = Players[p1].GetNextPos();
                
                foreach (var p2 in Players.Keys)
                {
                    if (!Players[p2].IsAlive() || p1 == p2) continue;
                    
                    p2Pos = conflictedPlayers.Contains(p2) ? Players[p2].GetPos() : Players[p2].GetNextPos();
                    
                    if (p1Pos != p2Pos) continue;

                    if (!conflictedPlayers.Contains(p1))
                    {
                        conflictedPlayers.Add(p1);
                        conflictedPlayersNumber++;
                    }

                    if (!conflictedPlayers.Contains(p2))
                    {
                        conflictedPlayers.Add(p2);
                        conflictedPlayersNumber++;
                    }
                }
            }
        } while (conflictedPlayersNumber != conflictedPlayers.Count);
        
        return conflictedPlayers;
    }

    private void Falls(Tile[,] tiles)
    {
        Vector2Int pos;

        foreach (string p in Players.Keys)
        {
            if (!Players[p].IsAlive()) continue;
            
            pos = Players[p].GetPos();

            if (tiles[pos.x, pos.y])
                Players[p].Fall();
        }
    }

    private void ResetMovements()
    {
        foreach (string p in Movements.Keys)
        {
            Movements[p] = Movement.None;
        }
    }

    public void SetMovement(string player, Movement move)
    {
        Movements[player] = move;
    }

    public void AddPlayer(string playerPseudo)
    {
        Players.Add(playerPseudo, null);
    }

    public void RemovePlayer(string playerPseudo)
    {
        if (Players.ContainsKey(playerPseudo))
            Players.Remove(playerPseudo);
    }

    public bool ContainsPlayer(string playerPseudo)
    {
        return Players.ContainsKey(playerPseudo);
    }

    public void SetUp()
    {
        Debug.Log("start players setup...");
        List<string> keys = new List<string>(Players.Keys);
        foreach (string playerName in keys)
        {
            Debug.Log("create player  : " + playerName);
            Player p = Instantiate(playerPrefab);
            p.name = playerName;
            Players[playerName] = p;
        }
        Debug.Log("...players setup done");
    }
}
