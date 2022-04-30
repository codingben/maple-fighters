using System;
using DotNetEnv;
using Fleck;
using Game.Application.Components;

Env.Load();

var url = Environment.GetEnvironmentVariable("URL");
var server = new WebSocketServer(url);
var serverComponents = new ComponentCollection(new IComponent[]
{
    new IdGenerator(),
    new WebSocketSessionCollection(),
    new GameSceneCollection(),
    new GameSceneManager(),
    new GameClientCollection()
});
var gameClientCollection = serverComponents.Get<IGameClientCollection>();

AppDomain.CurrentDomain.ProcessExit += (s, e) =>
{
    server?.Dispose();
    serverComponents?.Dispose();
};

server.Start((connection) => gameClientCollection.Add(connection));