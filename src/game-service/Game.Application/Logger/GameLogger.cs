using System;
using Serilog;

namespace Game.Logger
{
    public static class GameLogger
    {
        public static GameLogLevel GameLogLevel;

        public static void Debug(string message)
        {
            if (GameLogLevel >= GameLogLevel.Debug)
            {
                Log.Logger.Information("{0} [Debug] {2}", DateTime.Now, message);
            }
        }

        public static void Info(string message)
        {
            if (GameLogLevel >= GameLogLevel.Info)
            {
                Log.Logger.Information("{0} [Info] {2}", DateTime.Now, message);
            }
        }

        public static void Warning(string message)
        {
            if (GameLogLevel >= GameLogLevel.Warn)
            {
                Log.Logger.Information("{0} [Warning] {2}", DateTime.Now, message);
            }
        }

        public static void Error(string message)
        {
            if (GameLogLevel >= GameLogLevel.Error)
            {
                Log.Logger.Information("{0} [Error] {2}", DateTime.Now, message);
            }
        }
    }
}