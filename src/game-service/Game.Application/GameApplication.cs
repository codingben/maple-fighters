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

var fleckLog = Env.GetString("FLECK_LOG");
var imLog = Env.GetString("IM_LOG");
var gameLog = Env.GetString("GAME_LOG");
var maxConnections = Env.GetInt("MAX_CONNECTIONS");

FleckLog.LogAction = FleckLogAction.LogAction;
FleckLog.Level = (LogLevel)Enum.Parse(typeof(LogLevel), fleckLog);
InterestManagementLog.Level = (IMLogLevel)Enum.Parse(typeof(IMLogLevel), imLog);
GameLog.Level = (GameLogLevel)Enum.Parse(typeof(GameLogLevel), gameLog);

var url = Env.GetString("URL");
var server = new WebSocketServer(url);
var serverComponents = new ComponentCollection(new IComponent[]
{
    new IdGenerator(),
    new GameClientCollection(maxConnections),
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
    if (gameClientCollection.Add(gameClient) == false)
    {
        connection.Close();

        GameLog.Debug($"Client #{id} disconnected because server reached maximum number of clients.");
    }
});