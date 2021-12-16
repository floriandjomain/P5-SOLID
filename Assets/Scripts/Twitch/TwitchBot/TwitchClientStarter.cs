using UnityEngine;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

public class TwitchClientStarter : MonoBehaviour
{

    [SerializeField] private string _ip = "irc.chat.twitch.tv";
    [SerializeField] private int _port = 6667;
    [Space(10)]
    [SerializeField] private TwitchBotData _twitchBotData;
    [SerializeField] private GameState _gameState;
    [SerializeField] private TwitchChatMessageQueue _twitchChatMessageQueue;
    [Space(10)]
    [SerializeField] private StringVariable _channelName;

    private void Start()
    {
        Task.Run(TwitchThread);
        // TwitchThread();
    }

    private async void TwitchThread()
    {
        Debug.Log("[TwitchClientStarter] Start");
        TcpClient tcpClient = new TcpClient();
        await tcpClient.ConnectAsync(_ip, _port);

        await TwitchClientSender.InitializeAsync(tcpClient, _channelName.Value.ToLower(), _twitchBotData.Username, _twitchBotData.Password);

        await TwitchClientSender.SendConnectionMessageAsync(); // Connect you

        TwitchClientReader.Initialize(tcpClient);

        TwitchClientReader.OnMessage += TwitchClientReader_OnMessage;

        TwitchClientReader.StartReading();
    }

    private void TwitchClientReader_OnMessage(TwitchChatMessage twitchChatMessage)
    {
        // Passes the message to the queue if listening
        if (_gameState.GetState() == GameState.State.GameListening || _gameState.GetState() == GameState.State.LobbyListening)
        {
            //Debug.Log($"{twitchChatMessage.Sender} said '{twitchChatMessage.Message}'");
            _twitchChatMessageQueue.Enqueue(twitchChatMessage);
        }
    }

    private async void OnDisable()
    {
        TwitchClientReader.StopReading();
        // await TwitchClientSender.SendMessageAsync("I left (but not really I think, I dont know)");
    }
}
