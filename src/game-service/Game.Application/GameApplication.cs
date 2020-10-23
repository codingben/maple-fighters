using Common.ComponentModel;
using Common.Components;
using Game.Application.Components;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace Game.Application
{
    public class GameApplication : HttpServer, IServerApplication
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

            Log.Level = LogLevel.Info;
        }

        public void Startup()
        {
            AddWebSocketService("/game", () => new GameService(components));

            Start();

            if (IsListening)
            {
                Log.Info($"Server is running {Address:Port}");
            }
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