using System;
using DotNetEnv;
using Fleck;
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
    new GameSceneCollection(),
    new GameClientCollection(),
    new GameSceneManager()
});
var gameClientCollection = serverComponents.Get<IGameClientCollection>();

AppDomain.CurrentDomain.ProcessExit += (s, e) =>
{
    server?.Dispose();
    serverComponents?.Dispose();
};

server.Start((connection) => gameClientCollection.Add(connection));