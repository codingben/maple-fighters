using System;

namespace Game.Log
{
    public enum IMLogLevel
    {
        Debug,
        Info
    }

    public class InterestManagementLog : InterestManagement.ILogger
    {
        public static IMLogLevel Level;

        public void Info(string message)
        {
            if (Level == IMLogLevel.Info)
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