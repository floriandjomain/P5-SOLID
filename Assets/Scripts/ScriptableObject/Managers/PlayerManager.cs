using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Manager/Player")]
public class PlayerManager : ScriptableObject
{
    [SerializeField] private Dictionary<string, Player> Players = new Dictionary<string, Player>();
    [SerializeField] private ConflictManager _conflictManager;
    
    public event Action taarniendo;
    public event Action<string> onRemovedPlayer;

    [SerializeField] private Player playerPrefab;

    public Dictionary<string, Player> GetPlayers() => Players;
    public int GetCurrentPlayerNumber() => Players.Count;
    public int GetCurrentAlivePlayerNumber() => Players.Keys.Count(player => Players[player].IsAlive());

    public IEnumerator Turn()
    {
        yield return ApplyMovements(_conflictManager.ComputeConflicts(Players, GetAllAlivePlayersName()));
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

    public void RemoveAllMovementPlayer()
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

    public void AddPlayer(string playerPseudo, Vector2Int playerPosition, bool playerIsAlive)
    {
        Players.Add(playerPseudo, playerPrefab);
        Players[playerPseudo].Setup(playerPosition);
        if(!playerIsAlive) Players[playerPseudo].Fall();
    }

    public Player GetPlayerPrefab()
    {
        return playerPrefab;
    }

    public void Load(Dictionary<string, Player> playersObj)
    {
        Players = playersObj;
    }
}
