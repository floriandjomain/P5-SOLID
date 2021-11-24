using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public int nbPlayer = 0;
    public Player playerPrefab;

    public List<KeyBindings> playersKeyBindings;

    private List<Player> players;

    private void Start()
    {
        players = new List<Player>();
        CreatePlayers(nbPlayer);    
    }

    public void CreatePlayers(int nb)
    {
        for (int i = 0; i < nb; i++)
        {
            Player player = Instantiate(playerPrefab);
            player.keyBindings = playersKeyBindings[i];

            players.Add(player);
        }
    }
}
