using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Manager/Movement")]
public class MovementManager : ScriptableObject
{
    [SerializeField] private Dictionary<string, Movement> Movements = new Dictionary<string, Movement>();

    public void SetUp(PlayerManager playerManager)
    {
        foreach (string playerName in playerManager.GetPlayers().Keys)
        {
            Movements.Add(playerName, Movement.None);
            playerManager.onRemovedPlayer += RemoveMovement;
        }

        playerManager.taarniendo += ResetMovements;
        ResetMovements();
    }

    private void RemoveMovement(string playerName)
    {
        Movements.Remove(playerName);
    }

    private void ResetMovements()
    {
        List<string> moves = new List<string>(Movements.Keys);
        foreach (string p in moves)
            Movements[p] = Movement.None;
    }

    public Dictionary<string, Movement> GetMovements() => Movements;
    
    public void SetMovement(string player, Movement move)
    {
        Movements[player] = move;
    }
}