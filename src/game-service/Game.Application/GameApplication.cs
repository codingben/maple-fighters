using System;
using DotNetEnv;
using Fleck;
using Game.Application;
using Game.Application.Components;
using Game.Log;
using Serilog;

Env.Load();

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

FleckLog.LogAction = FleckLogAction.LogAction;
FleckLog.Level = (LogLevel)Enum.Parse(typeof(LogLevel), Env.GetString("FLECK_LOG"));
InterestManagementLog.Level = (IMLogLevel)Enum.Parse(typeof(IMLogLevel), Env.GetString("IM_LOG"));
GameLog.Level = (GameLogLevel)Enum.Parse(typeof(GameLogLevel), Env.GetString("GAME_LOG"));

var url = Env.GetString("URL");
var server = new WebSocketServer(url);
var serverComponents = new ComponentCollection(new IComponent[]
{
    new IdGenerator(),
    new GameClientCollection(),
    new GameSceneCollection(),
    new GameSceneManager()
});
var idGenerator = serverComponents.Get<IIdGenerator>();
var gameClientCollection = serverComponents.Get<IGameClientCollection>();
var gameSceneCollection = serverComponents.Get<IGameSceneCollection>();

AppDomain.CurrentDomain.ProcessExit += (_, _) =>
{
    server?.Dispose();
    serverComponents?.Dispose();
};

server.Start((connection) =>
{
    var id = idGenerator.GenerateId();
    var gameClient = new GameClient(
        id,
        connection,
        gameClientCollection,
        gameSceneCollection);

    gameClientCollection.Add(gameClient);
});