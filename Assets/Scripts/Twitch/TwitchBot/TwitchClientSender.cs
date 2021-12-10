using System.IO;
using System.Threading.Tasks;
using System.Net.Sockets;

public static class TwitchClientSender
{
    private static StreamWriter _streamWriter;
    private static string _channelName;

    public static async Task InitializeAsync(TcpClient tcpClient, string channelName, string nickname, string password)
    {
        _streamWriter = new StreamWriter(tcpClient.GetStream()) { NewLine = "\r\n", AutoFlush = true };
        _channelName = channelName; 

        await _streamWriter.WriteLineAsync($"PASS {password}");
        await _streamWriter.WriteLineAsync($"NICK {nickname}");
    }

    public static async Task SendMessageAsync(string message)
    {
        await _streamWriter.WriteLineAsync($"PRIVMSG #{_channelName} :{message}");
    }

    public static async Task SendConnectionMessageAsync()
    {
        await _streamWriter.WriteLineAsync($"JOIN #{_channelName}");

        await SendMessageAsync("My Bot Joined !");
    }

    public static async Task SendPongResponseAsync(string url)
    {
        await _streamWriter.WriteLineAsync($"PONG {url}");
    }
}