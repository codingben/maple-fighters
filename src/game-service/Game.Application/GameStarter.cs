using System;
using DotNetEnv;
using Game.Application;

Env.Load();

var ipAddress = Environment.GetEnvironmentVariable("IP_ADDRESS");
if (ipAddress == null)
{
    throw new Exception("Please set IP_ADDRESS");
}

var serverApplication = new GameApplication(ipAddress);
serverApplication.Startup();

AppDomain.CurrentDomain.ProcessExit += (x, y) => serverApplication?.Shutdown();