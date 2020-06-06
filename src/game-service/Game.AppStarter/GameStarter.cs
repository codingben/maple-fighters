using System;
using Game.Application;

namespace Game.AppStarter
{
    public static class GameStarter
    {
        public static void Main()
        {
            var serverApplication = GetServerApplication();
            serverApplication.Startup();

            Console.ReadLine();

            serverApplication.Shutdown();
        }

        private static IServerApplication GetServerApplication()
        {
            return new GameApplication("ws://localhost:50060");
        }
    }
}