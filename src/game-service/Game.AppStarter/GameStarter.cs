using System;
using Game.Application;

namespace Game.AppStarter
{
    public static class GameStarter
    {
        public static void Main()
        {
            var gameApplication = new GameApplication("ws://localhost:50060");
            gameApplication.Startup();

            Console.ReadLine();

            gameApplication.Shutdown();
        }
    }
}