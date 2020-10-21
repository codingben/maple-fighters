using System;
using DotNetEnv;
using Game.Application;

namespace Game.AppStarter
{
    public static class GameStarter
    {
        private static IServerApplication serverApplication;

        public static void Main()
        {
            Env.Load();

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
            AppDomain.CurrentDomain.ProcessExit += (x, y) => StopApplication();
        }

        private static IServerApplication CreateServerApplication()
        {
            var ipAddress = Environment.GetEnvironmentVariable("IP_ADDRESS");
            if (ipAddress == null)
            {
                throw new NullReferenceException("Please set IP_ADDRESS");
            }

            return new GameApplication(ipAddress);
        }
    }
}