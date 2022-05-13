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
        public static GameLogLevel GameLogLevel;

        public static void Debug(string message)
        {
            if (GameLogLevel >= GameLogLevel.Debug)
            {
                Serilog.Log.Logger.Information(
                    "{0} [{1}] {2}",
                    DateTime.Now,
                    GameLogLevel.Debug,
                    message);
            }
        }

        public static void Info(string message)
        {
            if (GameLogLevel >= GameLogLevel.Info)
            {
                Serilog.Log.Logger.Information(
                    "{0} [{1}] {2}",
                    DateTime.Now,
                    GameLogLevel.Info,
                    message);
            }
        }

        public static void Warning(string message)
        {
            if (GameLogLevel >= GameLogLevel.Warn)
            {
                Serilog.Log.Logger.Information(
                    "{0} [{1}] {2}",
                    DateTime.Now,
                    GameLogLevel.Warn,
                    message);
            }
        }

        public static void Error(string message)
        {
            if (GameLogLevel >= GameLogLevel.Error)
            {
                Serilog.Log.Logger.Information(
                    "{0} [{1}] {2}",
                    DateTime.Now,
                    GameLogLevel.Error,
                    message);
            }
        }
    }
}