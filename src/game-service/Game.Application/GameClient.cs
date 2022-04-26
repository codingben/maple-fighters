using System;
using Game.Application.Components;
using Fleck;

namespace Game.Application
{
    public class GameClient : IDisposable
    {
        private IComponents Components { get; }

        public GameClient(IWebSocketConnection connection, IComponents serverComponents)
        {
            var id = serverComponents.Get<IIdGenerator>().GenerateId();
            var sessionCollection = serverComponents.Get<IWebSocketSessionCollection>();
            var gameSceneCollection = serverComponents.Get<IGameSceneCollection>();

            Components = new ComponentCollection();
            Components.Add(new WebSocketConnectionProvider(id, connection, sessionCollection));
            Components.Add(new WebSocketConnectionErrorHandler());
            Components.Add(new RemotePlayerProvider());
            Components.Add(new RemotePlayerMessageSender(sessionCollection));
            Components.Add(new MessageHandlerManager(gameSceneCollection));

            Console.WriteLine($"A new client #{id} is connected.");
        }

        public void Dispose()
        {
            Components?.Dispose();
        }
    }
}