using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Manager/Player")]
public class PlayerManager : ScriptableObject
{
    [SerializeField] private Dictionary<string, Player> Players = new Dictionary<string, Player>();
    
    public event Action taarniendo;
    public event Action<string> onRemovedPlayer;

    [SerializeField] private Player playerPrefab;

    public Dictionary<string, Player> GetPlayers() => Players;
    public int GetCurrentPlayerNumber() => Players.Count;
    public int GetCurrentAlivePlayerNumber() => Players.Keys.Count(player => Players[player].IsAlive());

    public IEnumerator Turn()
    {
        yield return ApplyMovements(ComputeConflicts());
        taarniendo?.Invoke();
    }

    private IEnumerator ApplyMovements(List<string> conflictedPlayers)
    {
        List<Player> movingPlayers = new List<Player>();
        
        List<string> players = new List<string>(Players.Keys);
        foreach (string p in players)
        {
            if (!Players[p].IsAlive()) continue;
            
            if (!conflictedPlayers.Contains(p))
            {
                Debug.Log($"[PlayerManager] {p} apply movement");
                GameManager.Instance.StartPlayerCoroutine(Players[p].ApplyMovement());
                movingPlayers.Add(Players[p]);
            }
            else
            {
                Debug.Log($"[PlayerManager] {p} u-turn");
                
                if (!Players[p].WillUTurn()) continue;
                
                GameManager.Instance.StartPlayerCoroutine(Players[p].UTurn());
                movingPlayers.Add(Players[p]);
            }
        }

        yield return new WaitForSeconds(2);
        yield return WaitForPlayersToMove(movingPlayers);
    }

    private IEnumerator WaitForPlayersToMove(List<Player> movingPlayers)
    {
        bool ok;
        
        do
        {
            ok = true;
            
            foreach (Player p in movingPlayers)
            {
                if (p.IsMoving) ok = false;
            }
            
        } while (!ok);

        yield return null;
    }

    private List<string> ComputeConflicts()
    {
        List<string> conflictedPlayers = new List<string>();
        List<string> players = GetAllAlivePlayersName();
        
        foreach(string p1 in players)
        {
            //if won't move
            if (conflictedPlayers.Contains(p1) || Players[p1].GetPos() == Players[p1].GetNextPos()) continue;
            
            //get p1's positions
            Vector2Int p1Pos = Players[p1].GetPos();
            Vector2Int p1NextPos = Players[p1].GetNextPos();
            
            foreach(string p2 in players)
            {
                if (p1 == p2) continue;
                
                //get p2's positions
                Vector2Int p2Pos = Players[p2].GetPos();
                Vector2Int p2NextPos = Players[p2].GetNextPos();
                
                if (!conflictedPlayers.Contains(p2))
                {
                    if (p1Pos == p2NextPos && p2Pos == p1NextPos || p1NextPos == p2NextPos)
                    {
                        bool max = p1NextPos != p2NextPos;
                        AddToConflictList(ref conflictedPlayers, p2, max);
                        AddToConflictList(ref conflictedPlayers, p1, max);
                    }
                }
                else if (p1NextPos == p2Pos)
                    AddToConflictList(ref conflictedPlayers, p1, true);
            }
        }
        
        return conflictedPlayers;
    }

    private void AddToConflictList(ref List<string> list, string pName, bool _uTurnMax)
    {
        if (list.Contains(pName)) return;
                    
        list.Add(pName);
        Players[pName].SetUTurn(_uTurnMax);
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
    }

    public void RemovePlayer(string playerPseudo)
    {
        if (!Players.ContainsKey(playerPseudo)) return;
        
        Players.Remove(playerPseudo);
        onRemovedPlayer?.Invoke(playerPseudo);
    }

    public void RemoveAllPlayers()
    {
        List<string> keys = Players.Keys.ToList();
        foreach (string key in keys) 
        {
            RemovePlayer(key);
        }
    }

    public void RemoveAllMovmentPlayer()
    {
        foreach (string key in Players.Keys)
        {
            onRemovedPlayer?.Invoke(key);
        }
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
