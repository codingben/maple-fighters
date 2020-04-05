using System;
using CommonTools.Log;

namespace Game.Application
{
    public class Logger : ILogger
    {
        public void Log(string message, LogMessageType type = LogMessageType.Log, object context = null)
        {
            throw new NotImplementedException();
        }

        public void Break()
        {
            throw new NotImplementedException();
        }
    }
}