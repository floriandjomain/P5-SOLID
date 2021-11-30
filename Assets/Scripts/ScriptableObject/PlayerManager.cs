using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Manager/Player")]
public class PlayerManager : ScriptableObject
{
    [SerializeField] private Dictionary<string, Player> Players = new Dictionary<string, Player>();
    [SerializeField] private Dictionary<string, Movement> Movements = new Dictionary<string, Movement>();

    public Dictionary<string, Player> GetPlayers() => Players;
    public int GetCurrentPlayerNumber() => Players.Count;

    public void Turn(Tile[][] tiles)
    {
        CompileMovements(tiles);
        ApplyMovements();
        Falls(tiles);
        ResetMovements();
    }

    private void CompileMovements(Tile[][] tiles)
    {
        foreach (string p in Players.Keys)
        {
            CompileMovement(Players[p], Movements[p], tiles);
        }
    }

    private void CompileMovement(Player player, Movement movement, Tile[][] tiles)
    {
        Vector2Int pos = player.GetPos() + MovementManager.GetVector(movement);

        if (pos.x < 0 || pos.y < 0 || pos.x >= tiles.Length || pos.y >= tiles[0].Length) return;

        if (!tiles[pos.x][pos.y].isActiveAndEnabled) return;

        player.SetNextPos(pos);
    }

    private void ApplyMovements()
    {
        List<string> conflictedPlayers = new List<string>();

        foreach (string p1 in Players.Keys)
        {
            foreach (string p2 in Players.Keys)
            {
                if (p1 == p2 || Players[p1].GetNextPos() != Players[p2].GetNextPos()) continue;

                if (!conflictedPlayers.Contains(p1))
                    conflictedPlayers.Add(p1);
                if (!conflictedPlayers.Contains(p2))
                    conflictedPlayers.Add(p2);
            }
        }

        foreach (string p in Players.Keys)
        {
            if (!conflictedPlayers.Contains(p))
                Players[p].ApplyMovement();
        }
    }

    private void Falls(Tile[][] tiles)
    {
        Vector2Int pos;

        foreach (string p in Players.Keys)
        {
            if (Players[p].IsAlive())
            {
                pos = Players[p].GetPos();

                if (tiles[pos.x][pos.y])
                    Players[p].Fall();
            }
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
}
