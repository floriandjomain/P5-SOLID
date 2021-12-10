using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Manager/Player")]
public class PlayerManager : ScriptableObject
{
    [SerializeField] private Dictionary<string, Player> Players = new Dictionary<string, Player>();
    
    public event Action taarniendo;
    public event Action<string> onAddedPlayer;
    public event Action<string> onRemovedPlayer;

    [SerializeField] private Player playerPrefab;

    public Dictionary<string, Player> GetPlayers() => Players;
    public int GetCurrentPlayerNumber() => Players.Count;
    public int GetCurrentAlivePlayerNumber() => Players.Keys.Count(player => Players[player].IsAlive());

    public void Turn(Tile[,] tiles)
    {
        ApplyMovements();
        Falls(tiles);
        taarniendo?.Invoke();
    }

    private void ApplyMovements()
    {
        List<string> conflictedPlayers = CheckForConflicts();
        
        List<string> players = new List<string>(Players.Keys);
        foreach (string p in players)
        {
            if (!Players[p].IsAlive()) continue;
            
            if (!conflictedPlayers.Contains(p))
            {
                if(p == "flupiiipi") Debug.Log("[PlayerManager] apply movement");
                Players[p].ApplyMovement();
            }
            else
            {
                if(p == "flupiiipi") Debug.Log("[PlayerManager] u-turn");
                if(Players[p].willUTurn)
                    Players[p].UTurn();
            }
        }
    }

    private List<string> CheckForConflicts()
    {
        List<string> conflictedPlayers = new List<string>();
        int conflictedPlayersNumber = 0;

        do
        {
            List<string> players = new List<string>(Players.Keys);
            foreach(string p1 in players)
            {
                if (!Players[p1].IsAlive() || conflictedPlayers.Contains(p1) || Players[p1].GetPos() == Players[p1].GetNextPos()) continue;
                
                Vector2Int p1Pos = Players[p1].GetNextPos();
                
                foreach(string p2 in players)
                {
                    if (p1 == p2 || !Players[p2].IsAlive()) continue;
                    
                    Vector2Int p2Pos = conflictedPlayers.Contains(p2) ? Players[p2].GetPos() : Players[p2].GetNextPos();
                    
                    if (p1Pos != p2Pos) continue;

                    if (!conflictedPlayers.Contains(p1))
                    {
                        conflictedPlayers.Add(p1);
                        Players[p1].willUTurn=true;
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

    public void AddPlayer(string playerPseudo)
    {
        if(Players.ContainsKey(playerPseudo)) return;
        
        Players.Add(playerPseudo, null);
        onAddedPlayer?.Invoke(playerPseudo);
    }

    public void RemovePlayer(string playerPseudo)
    {
        if (!Players.ContainsKey(playerPseudo)) return;
        
        Players.Remove(playerPseudo);
        onRemovedPlayer?.Invoke(playerPseudo);
    }

    public bool ContainsPlayer(string playerPseudo)
    {
        return Players.ContainsKey(playerPseudo);
    }

    public void SetUp(float playTime)
    {
        //Debug.Log("[PlayerManager] start players setup...");
        PlayersInstantiation(playTime);
        //taarniendo += delegate { Debug.Log("[PlayerManager] terminééééééé"); };
        //Debug.Log("[PlayerManager] ...players setup done");
    }

    private void PlayersInstantiation(float playTime)
    {
        List<string> players = new List<string>(Players.Keys);
        foreach (string playerName in players)
        {
            //Debug.Log("[PlayerManager] create player  : " + playerName);
            Player p = Instantiate(playerPrefab, GameManager.Instance.PlayersGO.transform, true);
            p.name = playerName;
            p.PlayTime = playTime;
            Players[playerName] = p;
        }
    }

    public List<Vector3> GetAllAlivePlayersCapsulePosition()
    {
        List<Vector3> alivePlayersCapsulePosition = new List<Vector3>();

        foreach (string playerName in GetAllAlivePlayersName())
        {
            if(Players[playerName].IsAlive()) alivePlayersCapsulePosition.Add(Players[playerName].GetCapsulePos());
        }

        return alivePlayersCapsulePosition;
    }

    public List<Vector2Int> GetAllAlivePlayersPosition()
    {
        List<Vector2Int> positions = new List<Vector2Int>();

        foreach (string playerName in GetAllAlivePlayersName())
        {
            if(Players[playerName].IsAlive()) positions.Add(Players[playerName].GetPos());
        }

        return positions;
    }

    public List<string> GetAllAlivePlayersName()
    {
        List<string> alivePlayerNames = new List<string>();
        
        List<string> playerNames = Players.Keys.ToList();
        foreach (string playerName in playerNames)
        {
            if(Players[playerName].IsAlive()) alivePlayerNames.Add(playerName);
        }

        return alivePlayerNames;
    }
}
