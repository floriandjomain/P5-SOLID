using UnityEngine;
using System.Net.Sockets;

public class TwitchClientStarter : MonoBehaviour
{

    [SerializeField] private string ip = "irc.chat.twitch.tv";
    [SerializeField] private int port = 6667;
    [Space(10)]
    [SerializeField] private TwitchBotData twitchBotData;
    [SerializeField] private GameState gameState;
    [SerializeField] private TwitchChatMessageQueue twitchChatMessageQueue;
    [Space(10)]
    [SerializeField] private string channelName;

    private async void Start()
    {
        TcpClient tcpClient = new TcpClient();
        await tcpClient.ConnectAsync(ip, port);

        channelName = channelName.ToLower();

        await TwitchClientSender.Initialize(tcpClient, channelName, twitchBotData.Username, twitchBotData.Password);

        await TwitchClientSender.SendConnectionMessage(); // connect you

        TwitchClientReader.Initialize(tcpClient);

        TwitchClientReader.OnMessage += TwitchClientReader_OnMessage;

        TwitchClientReader.StartReading();
    }

    private void TwitchClientReader_OnMessage(TwitchChatMessage twitchChatMessage)
    {
        // Passes the message to the queue if listening
        if (gameState.GetState() == GameState.State.GameListening || gameState.GetState() == GameState.State.LobbyListening)
        {
            //Debug.Log($"{twitchChatMessage.Sender} said '{twitchChatMessage.Message}'");
            twitchChatMessageQueue.Enqueue(twitchChatMessage);
        }
    }
}
