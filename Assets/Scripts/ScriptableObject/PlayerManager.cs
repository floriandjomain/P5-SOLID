using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Manager/Player")]
public class PlayerManager : ScriptableObject
{
    [SerializeField] private Dictionary<string, Player> Players = new Dictionary<string, Player>();
    [SerializeField] private Dictionary<string, Movement> Movements = new Dictionary<string, Movement>();

    public event Action taarniendo;

    [SerializeField] private Player playerPrefab;

    public Dictionary<string, Player> GetPlayers() => Players;
    public int GetCurrentPlayerNumber() => Players.Count;

    public void Turn(Tile[,] tiles)
    {
        CompileMovements(tiles);
        ApplyMovements();
        Falls(tiles);
        ResetMovements();
        taarniendo?.Invoke();
    }

    private void CompileMovements(Tile[,] tiles)
    {
        List<string> players = new List<string>(Players.Keys);
        foreach (string p in players)
        {
            CompileMovement(Players[p], Movements[p], tiles);
        }
    }

    private void CompileMovement(Player player, Movement movement, Tile[,] tiles)
    {
        if(!player.IsAlive()) return;
        
        Vector2Int pos = player.GetPos() + MovementManager.GetVector(movement);

        if (pos.x < 0 || pos.y < 0 || pos.x >= tiles.GetLength(0) || pos.y >= tiles.GetLength(0) || tiles[pos.x, pos.y].IsBroken())
        {
            player.SetPos(pos);
            player.Fall();
            return;
        }

        player.SetNextPos(pos);
    }

    private void ApplyMovements()
    {
        List<string> conflictedPlayers = CheckForConflicts();
        
        List<string> players = new List<string>(Players.Keys);
        foreach (string p in players)
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
            List<string> players = new List<string>(Players.Keys);
            foreach (string p1 in players)
            {
                if(!Players[p1].IsAlive() || conflictedPlayers.Contains(p1)) continue;
                
                p1Pos = Players[p1].GetNextPos();
                
                foreach (string p2 in players)
                {
                    if (p1 == p2 || !Players[p2].IsAlive()) continue;
                    
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

    public void Falls(Tile[,] tiles)
    {
        Vector2Int pos;

        List<string> players = new List<string>(Players.Keys);
        foreach (string p in players)
        {
            if (!Players[p].IsAlive()) continue;
            
            pos = Players[p].GetPos();

            if (tiles[pos.x, pos.y].IsBroken()) Players[p].Fall();
        }
    }

    private void ResetMovements()
    {
        List<string> moves = new List<string>(Movements.Keys);
        foreach (string p in moves)
            Movements[p] = Movement.None;
    }

    public void SetMovement(string player, Movement move)
    {
        Movements[player] = move;
    }

    public void AddPlayer(string playerPseudo)
    {
        Players.Add(playerPseudo, null);
        Movements.Add(playerPseudo, Movement.None);
    }

    public void RemovePlayer(string playerPseudo)
    {
        if (Players.ContainsKey(playerPseudo))
        {
            Players.Remove(playerPseudo);
            Movements.Remove(playerPseudo);
        }
    }

    public bool ContainsPlayer(string playerPseudo)
    {
        return Players.ContainsKey(playerPseudo);
    }

    public void SetUp()
    {
        //Debug.Log("start players setup...");
        PlayersInstantiation();
        //taarniendo += (() => { Debug.Log("event called"); });
        //Debug.Log("...players setup done");
    }

    private void PlayersInstantiation()
    {
        List<string> players = new List<string>(Players.Keys);
        foreach (string playerName in players)
        {
            //Debug.Log("create player  : " + playerName);
            Player p = Instantiate(playerPrefab);
            p.name = playerName;
            Players[playerName] = p;
        }
    }

    public List<Vector3> GetAllAlivePlayersCapsulePosition()
    {
        List<Vector3> alivePlayersCapsulePosition = new List<Vector3>();

        List<string> players = new List<string>(Players.Keys);
        foreach (string playerName in players)
        {
            if(Players[playerName].IsAlive()) alivePlayersCapsulePosition.Add(Players[playerName].GetCapsulePos());
        }

        return alivePlayersCapsulePosition;
    }
}
