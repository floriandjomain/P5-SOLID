using UnityEngine;
using System.IO;
using System.Net.Sockets;
using System;
using System.Threading;
using System.Threading.Tasks;

public static class TwitchClientReader
{
    private static StreamReader _streamReader;

    public static event TwitchChatEventHandler OnMessage = delegate { };
    public delegate void TwitchChatEventHandler(TwitchChatMessage twitchChatMessage);

    private static bool _IsReading;
    private static CancellationTokenSource _cancellationTokenSource;

    public static void Initialize(TcpClient tcpClient)
    {
        _streamReader = new StreamReader(tcpClient.GetStream());
        _IsReading = true;
        _cancellationTokenSource = new CancellationTokenSource();
    }

    public static Task<T> WithCancellation<T>(this Task<T> task, CancellationToken cancellationToken)
    {
        return task.IsCompleted // fast-path optimization
            ? task
            : task.ContinueWith(
                completedTask => completedTask.GetAwaiter().GetResult(),
                cancellationToken,
                TaskContinuationOptions.ExecuteSynchronously,
                TaskScheduler.Default);
    }

    public static async void StartReading()
    {
        try
        {
            while (_IsReading)
            {
                string line = await _streamReader.ReadLineAsync().WithCancellation(_cancellationTokenSource.Token);

                string[] split = line.Split(' ');
                // if { PING :tmi.twitch.tv } => Respond with { PONG :tmi.twitch.tv }
                if (line.StartsWith("PING"))
                {
                    // Call the TwitchClientSender To Send That Message
                    await TwitchClientSender.SendPongResponseAsync(split[1]);
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
        catch (TaskCanceledException e)
        {
            Debug.Log("Caugth an TaskCanceledException in Thread [" + Thread.CurrentThread.Name + "]" + e.Message);
        }
        finally
        {
            _cancellationTokenSource.Dispose();
            _streamReader.Dispose();
        }
        
    }

    public static void StopReading()
    {
        _IsReading = false;
        _cancellationTokenSource.Cancel();
    }
}
