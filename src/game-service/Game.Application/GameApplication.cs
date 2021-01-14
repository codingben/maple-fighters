using Common.ComponentModel;
using Common.Components;
using Fleck;
using Game.Application.Components;

namespace Game.Application
{
    public class GameApplication : IServerApplication
    {
        private readonly IWebSocketServer webSocketServer;
        private readonly IComponents components;

        public GameApplication(string url)
        {
            webSocketServer = new WebSocketServer(url);
            components = new ComponentCollection(new IComponent[]
            {
                new IdGenerator(),
                new WebSocketSessionCollection(),
                new GameSceneCollection(),
                new GameSceneManager()
            });
        }

        public void Startup()
        {
            webSocketServer?.Start((config) => new GameService(config, components));
        }

        public void Shutdown()
        {
            webSocketServer?.Dispose();
            components?.Dispose();
        }
    }
}