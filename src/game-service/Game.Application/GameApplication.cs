using WebSocketSharp.Server;

namespace Game.Application
{
    public class GameApplication
    {
        WebSocketServer webSocketServer;

        public GameApplication(string url)
        {
            webSocketServer = new WebSocketServer(url);
        }

        public void Startup()
        {
            webSocketServer.AddWebSocketService<GameService>("/game");
            webSocketServer.Start();
        }

        public void Shutdown()
        {
            webSocketServer.Stop();
        }
    }
}