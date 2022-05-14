using System;

namespace Game.Log
{
    public enum GameLogLevel
    {
        Debug,
        Info,
        Warn,
        Error
    }

    public class GameLog
    {
        public static GameLogLevel LogLevel;

        public static Action<GameLogLevel, string> LogAction = (level, message) =>
        {
            if (LogLevel >= level)
            {
                Serilog.Log.Logger.Information(
                    "{0} [{1}] {2}",
                    DateTime.Now,
                    GameLogLevel.Debug,
                    message);
            }
        };

        public static void Debug(string message)
        {
            LogAction(GameLogLevel.Debug, message);
        }

        public static void Info(string message)
        {
            LogAction(GameLogLevel.Info, message);
        }

        public static void Warning(string message)
        {
            LogAction(GameLogLevel.Warn, message);
        }

        public static void Error(string message)
        {
            LogAction(GameLogLevel.Error, message);
        }
    }
}