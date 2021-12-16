using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Manager/Player")]
public class PlayerManager : ScriptableObject
{
    [SerializeField] private Dictionary<string, Player> _players = new Dictionary<string, Player>();
    [SerializeField] private ConflictManager _conflictManager;
    
    public event Action taarniendo;
    public event Action<string> onRemovedPlayer;

    [SerializeField] private Player playerPrefab;
    [SerializeField] public Player PlayerPrefab { get => playerPrefab; }

    public Dictionary<string, Player> GetPlayers() => _players;
    public int GetCurrentPlayerNumber() => _players.Count;
    public int GetCurrentAlivePlayerNumber() => _players.Keys.Count(player => _players[player].IsAlive);

    public List<Vector3> GetAllAlivePlayersCapsulePosition()
    {
        List<Vector3> alivePlayersCapsulePosition = new List<Vector3>();

        foreach (string playerName in GetAllAlivePlayersName())
        {
            if(_players[playerName].IsAlive) alivePlayersCapsulePosition.Add(_players[playerName].GetCapsulePos());
        }

        return alivePlayersCapsulePosition;
    }

    public List<Vector2Int> GetAllAlivePlayersPosition()
    {
        List<Vector2Int> positions = new List<Vector2Int>();

        foreach (string playerName in GetAllAlivePlayersName())
        {
            if(_players[playerName].IsAlive) positions.Add(_players[playerName].Position);
        }

        return positions;
    }

    public List<string> GetAllAlivePlayersName()
    {
        List<string> alivePlayerNames = new List<string>();
        
        List<string> playerNames = _players.Keys.ToList();
        foreach (string playerName in playerNames)
        {
            if(_players[playerName].IsAlive) alivePlayerNames.Add(playerName);
        }

        return alivePlayerNames;
    }

    public IEnumerator Turn()
    {
        yield return ApplyMovements(_conflictManager.ComputeConflicts(_players, GetAllAlivePlayersName()));
        taarniendo?.Invoke();
    }

    private IEnumerator ApplyMovements(List<string> conflictedPlayers)
    {
        List<Player> movingPlayers = new List<Player>();
        
        List<string> players = new List<string>(_players.Keys);
        foreach (string p in players)
        {
            if (!_players[p].IsAlive) continue;
            
            if (!conflictedPlayers.Contains(p))
            {
                Debug.Log($"[PlayerManager] {p} apply movement");
                GameManager.Instance.StartPlayerCoroutine(_players[p].ApplyMovement());
                movingPlayers.Add(_players[p]);
            }
            else
            {
                Debug.Log($"[PlayerManager] {p} u-turn");
                
                if (!_players[p].WillUTurn) continue;
                
                GameManager.Instance.StartPlayerCoroutine(_players[p].UTurn());
                movingPlayers.Add(_players[p]);
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

        List<string> players = new List<string>(_players.Keys);
        foreach (string p in players)
        {
            if (!_players[p].IsAlive) continue;
            
            pos = _players[p].Position;

            if (tiles[pos.x, pos.y].IsBroken()) _players[p].Fall();
        }
    }

    public void AddPlayer(string playerPseudo)
    {
        if(_players.ContainsKey(playerPseudo)) return;
        
        _players.Add(playerPseudo, null);
    }

    public void RemovePlayer(string playerPseudo)
    {
        if (!_players.ContainsKey(playerPseudo)) return;
        
        _players.Remove(playerPseudo);
        onRemovedPlayer?.Invoke(playerPseudo);
    }

    public void RemoveAllPlayers()
    {
        List<string> keys = _players.Keys.ToList();
        foreach (string key in keys) 
        {
            RemovePlayer(key);
        }
    }

    public void RemoveAllMovementPlayer()
    {
        foreach (string key in _players.Keys)
        {
            onRemovedPlayer?.Invoke(key);
        }
    }

    public bool ContainsPlayer(string playerPseudo)
    {
        return _players.ContainsKey(playerPseudo);
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
        List<string> players = new List<string>(_players.Keys);
        foreach (string playerName in players)
        {
            //Debug.Log("[PlayerManager] create player  : " + playerName);
            Player p = Instantiate(playerPrefab, GameManager.Instance.PlayersGO.transform, true);
            p.name = playerName;
            p.PlayTime = playTime;
            _players[playerName] = p;
        }
    }

    public void AddPlayer(string playerPseudo, Vector2Int playerPosition, bool playerIsAlive)
    {
        _players.Add(playerPseudo, playerPrefab);
        _players[playerPseudo].Setup(playerPosition);
        if(!playerIsAlive) _players[playerPseudo].Fall();
    }

    public void Load(Dictionary<string, Player> playersObj)
    {
        _players = playersObj;
    }
}
