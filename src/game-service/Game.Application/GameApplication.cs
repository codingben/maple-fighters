using System;
using Common.ComponentModel;
using Common.Components;
using DotNetEnv;
using Fleck;
using Game.Application;
using Game.Application.Components;

Env.Load();

var url = Environment.GetEnvironmentVariable("URL");
var server = new WebSocketServer(url);
var components = new ComponentCollection(new IComponent[]
{
    new IdGenerator(),
    new WebSocketSessionCollection(),
    new GameSceneCollection(),
    new GameSceneManager()
});

AppDomain.CurrentDomain.ProcessExit += (s, e) =>
{
    server?.Dispose();
    components?.Dispose();
};

server.Start((connection) => new GameService(connection, components));