using Common.ComponentModel;
using Common.Components;
using Game.Application.Components;
using WebSocketSharp.Server;

namespace Game.Application
{
    public class GameApplication : WebSocketServer, IServerApplication
    {
        private readonly ComponentCollection components;

        public GameApplication(string url)
            : base(url)
        {
            var collection = new IComponent[]
            {
                new IdGenerator(),
                new WebSocketSessionCollection(),
                new GameSceneCollection(),
                new GameSceneManager()
            };

            components = new ComponentCollection(collection);
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