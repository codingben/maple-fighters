using System;
using DotNetEnv;
using Fleck;
using Game.Application;
using Game.Application.Components;
using Game.Logger;
using Serilog;

Env.Load();

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

FleckLog.LogAction = (level, message, exception) =>
{
    if (level >= LogLevel.Info)
    {
        if (exception == null)
            Log.Logger.Information("{0} [{1}] {2}", DateTime.Now, level, message);
        else
            Log.Logger.Information("{0} [{1}] {2} {3}", DateTime.Now, level, message, exception);
    }
};

InterestManagementLogger.LogLevel = InterestMgmtLogLevel.Debug;
GameLogger.GameLogLevel = GameLogLevel.Debug;

var url = Environment.GetEnvironmentVariable("URL");
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
    var gameClient = new GameClient(id, connection, gameSceneCollection);

    gameClientCollection.Add(gameClient);
});