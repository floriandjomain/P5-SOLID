using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Dictionary/Player")]
public class DictionaryPlayer : ScriptableObject
{
    [SerializeField] Dictionary<string, Player> players = new Dictionary<string, Player>();

    public void AddPlayer(string player)
    {
        players.Add(player, null);
        DisplayList();
    }

    public void RemovePlayer(string player)
    {
        players.Remove(player);
        DisplayList();
    }

    public int GetNumberPlayer()
    {
        return players.Count;
    }

    public bool Contains(string player)
    {
        return players.ContainsKey(player);
    }

    private void DisplayList()
    {
        foreach(KeyValuePair<string, Player> player in players)
        {
            Debug.Log(player.Key);
        }
    }
}
