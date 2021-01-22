using System;
using Common.ComponentModel;
using Common.Components;
using DotNetEnv;
using Fleck;
using Game.Application.Components;

namespace Game.Application
{
    public class GameApplication : IServerApplication
    {
        private readonly IWebSocketServer webSocketServer;
        private readonly IComponents components;

        public GameApplication()
        {
            Env.Load();

            var url = Environment.GetEnvironmentVariable("URL");

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