using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Net.Sockets;
using UnityEngine;

// https://medium.com/swlh/writing-a-twitch-bot-from-scratch-in-c-f59d9fed10f3

public class TwitchClient : MonoBehaviour
{
    public string password = "oauth:aa0q3susodtaunnxwcdy1fo9l5xk7m";
    public string botUsername = "bot_projet5";
    public string channelName = "algergildartz";

    private async void Start()
    {
        var twitchBot = new TwitchBot(botUsername, password);
        twitchBot.Start();
        await twitchBot.JoinChannel(channelName);
        await twitchBot.SendMessage(channelName, "Hey my bot has started up");

        twitchBot.OnMessage += async (sender, twitchChatMessage) =>
        {
            Debug.Log($"{twitchChatMessage.Sender} said '{twitchChatMessage.Message}'");
            // Listen for !hey command
            if (twitchChatMessage.Message.StartsWith("!hey"))
            {
                await twitchBot.SendMessage(twitchChatMessage.Channel, $"Hey there {twitchChatMessage.Sender}");
            }
        };

        await Task.Delay(-1);
    }
}

public class TwitchBot
{
    const string ip = "irc.chat.twitch.tv";
    const int port = 6667;

    private string nick;
    private string password;
    private StreamReader streamReader;
    private StreamWriter streamWriter;
    private TaskCompletionSource<int> connected = new TaskCompletionSource<int>();

    public event TwitchChatEventHandler OnMessage = delegate { };
    public delegate void TwitchChatEventHandler(object sender, TwitchChatMessage e);

    public class TwitchChatMessage : EventArgs
    {
        public string Sender { get; set; }
        public string Message { get; set; }
        public string Channel { get; set; }
    }

    public TwitchBot(string nick, string password)
    {
        this.nick = nick;
        this.password = password;
    }

    public async Task Start()
    {
        var tcpClient = new TcpClient();
        await tcpClient.ConnectAsync(ip, port);
        streamReader = new StreamReader(tcpClient.GetStream());
        streamWriter = new StreamWriter(tcpClient.GetStream()) { NewLine = "\r\n", AutoFlush = true };

        await streamWriter.WriteLineAsync($"PASS {password}");
        await streamWriter.WriteLineAsync($"NICK {nick}");
        connected.SetResult(0);

        while (true)
        {
            string line = await streamReader.ReadLineAsync();
            Debug.Log(line);

            string[] split = line.Split(' ');
            //PING :tmi.twitch.tv
            //Respond with PONG :tmi.twitch.tv
            if (line.StartsWith("PING"))
            {
                Debug.Log("PONG");
                await streamWriter.WriteLineAsync($"PONG {split[1]}");
            }

            if (split.Length > 2 && split[1] == "PRIVMSG")
            {
                //:mytwitchchannel!mytwitchchannel@mytwitchchannel.tmi.twitch.tv 
                // ^^^^^^^^
                //Grab this name here
                int exclamationPointPosition = split[0].IndexOf("!");
                string username = split[0].Substring(1, exclamationPointPosition - 1);
                //Skip the first character, the first colon, then find the next colon
                int secondColonPosition = line.IndexOf(':', 1);//the 1 here is what skips the first character
                string message = line.Substring(secondColonPosition + 1);//Everything past the second colon
                string channel = split[2].TrimStart('#');

                OnMessage(this, new TwitchChatMessage
                {
                    Message = message,
                    Sender = username,
                    Channel = channel
                });
            }
        }
    }

    public async Task SendMessage(string channel, string message)
    {
        await connected.Task;
        await streamWriter.WriteLineAsync($"PRIVMSG #{channel} :{message}");
    }

    public async Task JoinChannel(string channel)
    {
        await connected.Task;
        await streamWriter.WriteLineAsync($"JOIN #{channel}");
    }
}
