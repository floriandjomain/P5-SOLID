using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Manager/Movement")]
public class MovementManager : ScriptableObject
{
    [SerializeField] private Dictionary<string, Movement> Movements = new Dictionary<string, Movement>();

    public void SetUp(PlayerManager playerManager)
    {
        playerManager.taarniendo += ResetMovements;
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

    public void AddPlayer(string playerPseudo)
    {
        Movements.Add(playerPseudo, Movement.None);
    }

    public void RemovePlayer(string playerPseudo)
    {
        if (Movements.ContainsKey(playerPseudo))
        {
            Movements.Remove(playerPseudo);
        }
    }
}
