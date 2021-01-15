using WebSocketSharp;

namespace Game.AppTests
{
    public class Program
    {
        public static void Main()
        {
            using (var webSocket = new WebSocket("ws://localhost:50051"))
            {
                webSocket.Log.Level = LogLevel.Info;
                webSocket.OnOpen += (x, y) => webSocket.Log.Info("OnOpen()");
                webSocket.OnError += (x, y) => webSocket.Log.Info("OnError()");
                webSocket.OnClose += (x, y) => webSocket.Log.Info("OnClose()");
                webSocket.Connect();
            }
        }
    }
}