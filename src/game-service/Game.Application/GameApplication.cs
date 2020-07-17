using Common.ComponentModel;
using Common.Components;
using Game.Application.Components;
using WebSocketSharp.Server;

namespace Game.Application
{
    public class GameApplication : WebSocketServer, IServerApplication
    {
        private readonly IComponents components;

        public GameApplication(string url)
            : base(url)
        {
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
            AddWebSocketService("/game", () => new GameService(components));

            Start();
        }

        public void Shutdown()
        {
            RemoveComponents();
            RemoveWebSocketService("/game");

            Stop();
        }

        private void RemoveComponents()
        {
            components?.Dispose();
        }
    }
}