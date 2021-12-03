using System.IO;
using System.Threading.Tasks;
using System.Net.Sockets;

public static class TwitchClientSender
{
    private static StreamWriter _streamWriter;
    private static string _channelName;

    public static async Task Initialize(TcpClient tcpClient, string channelName, string nickname, string password)
    {
        _streamWriter = new StreamWriter(tcpClient.GetStream()) { NewLine = "\r\n", AutoFlush = true };
        _channelName = channelName; 

        await _streamWriter.WriteLineAsync($"PASS {password}");
        await _streamWriter.WriteLineAsync($"NICK {nickname}");
    }

    public static async Task SendMessage(string message)
    {
        await _streamWriter.WriteLineAsync($"PRIVMSG #{_channelName} :{message}");
    }

    public static async Task SendConnectionMessage()
    {
        await _streamWriter.WriteLineAsync($"JOIN #{_channelName}");

        await SendMessage("My Bot Joined !");
    }

    public static async Task SendPongResponse(string url)
    {
        await _streamWriter.WriteLineAsync($"PONG {url}");
    }
}