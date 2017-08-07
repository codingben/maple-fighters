using ExitGames.Logging;

namespace Game.Application
{
    public static class Logger
    {
        public static ILogger Log => LogManager.GetCurrentClassLogger();
    }
}