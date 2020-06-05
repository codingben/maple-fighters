using Common.ComponentModel;
using Common.Components;
using WebSocketSharp.Server;

namespace Game.Application
{
    public class GameApplication
    {
        private readonly WebSocketServer webSocketServer;
        private readonly IExposedComponents components;

        public GameApplication(string url)
        {
            webSocketServer = new WebSocketServer(url);
            components = new ComponentsContainer();
        }

        public void Startup()
        {
            AddCommonComponents();

            webSocketServer.AddWebSocketService("/game", () => new GameService(components));
            webSocketServer.Start();
        }

        public void Shutdown()
        {
            webSocketServer.Stop();
        }

        private void AddCommonComponents()
        {
            components.Add(new IdGenerator());
        }
    }
}