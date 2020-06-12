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
            components = new ComponentsContainer();
        }

        public void Startup()
        {
            AddCommonComponents();
            AddWebSocketService("/game", () => new GameService((IExposedComponents)components));

            Start();
        }

        public void Shutdown()
        {
            RemoveCommonComponents();
            RemoveWebSocketService("/game");

            Stop();
        }

        private void AddCommonComponents()
        {
            components.Add(new IdGenerator());
            components.Add(new GameSceneContainer());
            components.Add(new SessionDataContainer());
        }

        private void RemoveCommonComponents()
        {
            components.Dispose();
        }
    }
}