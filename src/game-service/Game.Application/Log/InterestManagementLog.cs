using System;

namespace Game.Log
{
    public enum InterestManagementLogLevel
    {
        Debug,
        Info
    }

    public class InterestManagementLog : InterestManagement.ILogger
    {
        public static InterestManagementLogLevel LogLevel;

        public static Action<InterestManagementLogLevel, string> LogAction = (level, message) =>
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

        public void Info(string message)
        {
            LogAction(InterestManagementLogLevel.Info, message);
        }
    }
}