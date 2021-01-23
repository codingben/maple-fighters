using System;
using Common.ComponentModel;
using Common.Components;
using DotNetEnv;
using Fleck;
using Game.Application;
using Game.Application.Components;

Env.Load();

var url = Environment.GetEnvironmentVariable("URL");
var components = new ComponentCollection(new IComponent[]
{
    new IdGenerator(),
    new WebSocketSessionCollection(),
    new GameSceneCollection(),
    new GameSceneManager()
});

var webSocketServer = new WebSocketServer(url);
webSocketServer.Start((connection) => new GameService(connection, components));

AppDomain.CurrentDomain.ProcessExit += (s, e) =>
{
    components?.Dispose();
    webSocketServer?.Dispose();
};