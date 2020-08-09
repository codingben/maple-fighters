using System;
using Game.Application;

namespace Game.AppStarter
{
    public static class GameStarter
    {
        public static void Main()
        {
            // TODO: Docker
            var serverApplication = GetServerApplication();
            serverApplication.Startup();

            // TODO: Remove
            Console.ReadLine();

            // TODO: Remove
            serverApplication.Shutdown();
        }

        private static IServerApplication GetServerApplication()
        {
            return new GameApplication("ws://localhost:50060");
        }
    }
}