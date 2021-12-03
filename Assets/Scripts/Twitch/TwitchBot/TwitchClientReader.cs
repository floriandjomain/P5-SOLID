using UnityEngine;
using System.IO;
using System.Net.Sockets;

public static class TwitchClientReader
{
    private static StreamReader _streamReader;

    public static event TwitchChatEventHandler OnMessage = delegate { };
    public delegate void TwitchChatEventHandler(TwitchChatMessage twitchChatMessage);

    public static void Initialize(TcpClient tcpClient)
    {
        _streamReader = new StreamReader(tcpClient.GetStream());
    }

    public static async void StartReading()
    {
        while (true)
        {
            string line = await _streamReader.ReadLineAsync();
            
            string[] split = line.Split(' ');
            // if { PING :tmi.twitch.tv } => Respond with { PONG :tmi.twitch.tv }
            if (line.StartsWith("PING"))
            {
                Debug.Log("PONG");
                // Call the TwitchClientSender To Send That Message
                await TwitchClientSender.SendPongResponse(split[1]);
            }

            if (split.Length > 2 && split[1] == "PRIVMSG")
            {
                // line => { :username!username@username.tmi.twitch.tv PRIVMSG #channel :hello }
                int exclamationPointPosition = split[0].IndexOf("!");
                string username = split[0].Substring(1, exclamationPointPosition - 1);
                string channel = split[2].TrimStart('#');
                string message = split[3].TrimStart(':');
                
                OnMessage(new TwitchChatMessage
                {
                    Message = message,
                    Sender = username
                });
            }
            
        }
    }

}
