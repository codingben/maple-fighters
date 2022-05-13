using System;
using Serilog;

namespace Game.Logger
{
    public class GameLogger
    {
        public static GameLogLevel GameLogLevel;

        public static void Debug(string message)
        {
            if (GameLogLevel >= GameLogLevel.Debug)
            {
                Log.Logger.Information("{0} [{1}] {2}", DateTime.Now, GameLogLevel.Debug, message);
            }
        }

        public static void Info(string message)
        {
            if (GameLogLevel >= GameLogLevel.Info)
            {
                Log.Logger.Information("{0} [{1}] {2}", DateTime.Now, GameLogLevel.Info, message);
            }
        }

        public static void Warning(string message)
        {
            if (GameLogLevel >= GameLogLevel.Warn)
            {
                Log.Logger.Information("{0} [{1}] {2}", DateTime.Now, GameLogLevel.Warn, message);
            }
        }

        public static void Error(string message)
        {
            if (GameLogLevel >= GameLogLevel.Error)
            {
                Log.Logger.Information("{0} [{1}] {2}", DateTime.Now, GameLogLevel.Error, message);
            }
        }
    }
}