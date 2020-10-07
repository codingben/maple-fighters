using System;
using Game.Application;

namespace Game.AppStarter
{
    public static class GameStarter
    {
        private static IServerApplication serverApplication;

        public static void Main()
        {
            AddProcessExitEventHandler();

            StartApplication();
        }

        private static void StartApplication()
        {
            serverApplication = CreateServerApplication();
            serverApplication.Startup();
        }

        private static void StopApplication()
        {
            serverApplication?.Shutdown();
        }

        private static void AddProcessExitEventHandler()
        {
            // It's supposed to work with a docker container:
            AppDomain.CurrentDomain.ProcessExit += (x, y) => StopApplication();
        }

        private static IServerApplication CreateServerApplication()
        {
            // TODO: Get IP address from config
            return new GameApplication("ws://localhost:50051");
        }
    }
}