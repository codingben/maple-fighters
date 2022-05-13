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

        public static InterestManagement.ILogger GetLog()
        {
            if (logger == null)
            {
                logger = new InterestManagementLog();
            }

            return logger;
        }

        private static InterestManagementLog logger;

        public void Info(string message)
        {
            if (LogLevel >= InterestManagementLogLevel.Info)
            {
                Serilog.Log.Logger.Information(
                    "{0} [{1}] {2}",
                    DateTime.Now,
                    InterestManagementLogLevel.Info,
                    message);
            }
        }
    }
}