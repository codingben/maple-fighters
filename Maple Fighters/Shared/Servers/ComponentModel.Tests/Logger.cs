using CommonTools.Log;

namespace ComponentModel.Tests
{
    public class Logger : ILogger
    {
        public void Log(string message, LogMessageType type = LogMessageType.Log, object context = null)
        {
            // Left blank intentionally
        }

        public void Break()
        {
            // Left blank intentionally
        }
    }
}