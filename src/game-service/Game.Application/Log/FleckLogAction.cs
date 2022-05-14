using System;
using Fleck;

namespace Game.Log
{
    public class FleckLogAction
    {
        public static void LogAction(LogLevel level, string message, Exception exception)
        {
            if (level >= FleckLog.Level)
            {
                Serilog.Log.Logger.Information(
                    "{0} [{1}] {2} {3}",
                    DateTime.Now,
                    level,
                    message,
                    exception == null ? string.Empty : exception);
            }
        }
    }
}