using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.IO;
using UnityEngine;

public class TwitchClientTest : MonoBehaviour
{
    private TcpClient twitchClient;

    private StreamReader reader;
    private StreamWriter writer;

    public string username;
    public string password;
    public string channelName;

    private GameObject playerObject;

    private List<GameObject> users;
    private GameObject go;

    private int randomNumber;

    public List<string> usersInBattle = new List<string>();

    private bool isRegisteredChecked;

    private float randomTimer = 0.0f;

    private void Start()
    {
        randomTimer = 30.0f;
        Connect();
        users = new List<GameObject>();
        playerObject = GameObject.Find("user");

        if (playerObject != null) Debug.Log("Found user Object");
        else Debug.Log("No user Object found");
    }

    private void Update()
    {
        if (!twitchClient.Connected) Connect();

        ReadChat();

        randomTimer -= Time.deltaTime;

        if(randomTimer <= 0.0f)
        {
            SendPublicChatMessage("Hello from the bot");
        }
    }

    private void Connect()
    {
        twitchClient = new TcpClient("irc.chat.twitch.tv", 6667);
        reader = new StreamReader(twitchClient.GetStream());
        writer = new StreamWriter(twitchClient.GetStream()) { NewLine = "\r\n", AutoFlush = true };

        writer.WriteLine("PASS" + password);
        writer.WriteLine("NICK" + username);
        writer.WriteLine("USER" + username + " 8 * : " + username);
        writer.WriteLine("JOIN #" + channelName);
        writer.Flush();
    }

    private void ReadChat()
    {
        if(twitchClient.Available > 0)
        {
            string message = reader.ReadLine();

            if (message.Contains("PRIVMSG"))
            {
                // var splitPoint = message.
            }
        }
    }

    private void SendPublicChatMessage(string message)
    {

    }
}