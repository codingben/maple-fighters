using System;
using Game.Application;

var gameApplication = new GameApplication();
gameApplication.Startup();

AppDomain.CurrentDomain.ProcessExit += (s, e) => gameApplication?.Shutdown();