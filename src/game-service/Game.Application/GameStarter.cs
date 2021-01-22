using System;

namespace Game.Application
{
    public class GameStarter
    {
        public static void Main()
        {
            var gameApplication = new GameApplication();
            gameApplication.Startup();

            AppDomain.CurrentDomain.ProcessExit += (s, e) => gameApplication?.Shutdown();
        }
    }
}