using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Manager/Conflict")]
public class ConflictManager : ScriptableObject
{
    public List<string> ComputeConflicts(Dictionary<string, Player> players, List<string> alivePlayersName)
    {
        List<string> conflictedPlayers = new List<string>();
        
        foreach(string p1Name in alivePlayersName)
        {
            //if won't move
            if (conflictedPlayers.Contains(p1Name) || players[p1Name].Position == players[p1Name].NextPosition) continue;
            
            //get p1's positions
            Vector2Int p1Pos = players[p1Name].Position;
            Vector2Int p1NextPos = players[p1Name].NextPosition;
            
            foreach(string p2Name in alivePlayersName)
            {
                if (p1Name == p2Name) continue;
                
                //get p2's positions
                Vector2Int p2Pos = players[p2Name].Position;
                Vector2Int p2NextPos = players[p2Name].NextPosition;
                
                if (!conflictedPlayers.Contains(p2Name))
                {
                    if (p1Pos == p2NextPos && p2Pos == p1NextPos || p1NextPos == p2NextPos)
                    {
                        bool max = p1NextPos != p2NextPos;
                        AddToConflictList(ref conflictedPlayers, p2Name, players[p2Name], max);
                        AddToConflictList(ref conflictedPlayers, p1Name, players[p1Name], max);
                    }
                }
                else if (p1NextPos == p2Pos)
                    AddToConflictList(ref conflictedPlayers, p1Name, players[p2Name], true);
            }
        }
        
        return conflictedPlayers;
    }

    private void AddToConflictList(ref List<string> list, string pName, Player p, bool _uTurnMax)
    {
        if (list.Contains(pName)) return;
                    
        list.Add(pName);
        p.SetUTurn(_uTurnMax);
    }
}