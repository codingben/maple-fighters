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
        public static InterestManagementLogLevel Level;

        public void Info(string message)
        {
            if (Level == InterestManagementLogLevel.Info)
            {
                Serilog.Log.Logger.Information(
                    "{0} [{1}] {2}",
                    DateTime.Now,
                    Level,
                    message);
            }
        }
    }
}