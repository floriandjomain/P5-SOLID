using UnityEngine;
using System.Net.Sockets;
using System.Threading;

public class TwitchClientStarter : MonoBehaviour
{

    [SerializeField] private string ip = "irc.chat.twitch.tv";
    [SerializeField] private int port = 6667;
    [Space(10)]
    [SerializeField] private TwitchBotData twitchBotData;
    [SerializeField] private GameState gameState;
    [SerializeField] private TwitchChatMessageQueue twitchChatMessageQueue;
    [Space(10)]
    [SerializeField] private StringVariable channelName;

    private TcpClient _tcpClient;


    private async void Start()
    {
        Debug.Log("[TwitchClientStarter] Start");
        _tcpClient = new TcpClient();
        await _tcpClient.ConnectAsync(ip, port);

        await TwitchClientSender.InitializeAsync(_tcpClient, channelName.Value.ToLower(), twitchBotData.Username, twitchBotData.Password);

        await TwitchClientSender.SendConnectionMessageAsync(); // Connect you

        TwitchClientReader.Initialize(_tcpClient);

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

    private async void OnDisable()
    {
        TwitchClientReader.StopReading();
        await TwitchClientSender.SendMessageAsync("I left (but not really I think, I dont know)");
        
        Thread.Sleep(1000);

        _tcpClient.Close(); // Raise an error in [TwitchClientReader]
    }

}
