using CommonTools.Log;
using log4net;

namespace ServerCommon.Logging
{
    public class Logger : ILogger
    {
        private readonly ILog logger;
        private readonly object locker = new object();

        public Logger()
        {
            logger = LogManager.GetLogger(typeof(Logger));
        }

        public void Log(
            string message,
            LogMessageType type = LogMessageType.Log,
            object context = null)
        {
            lock (locker)
            {
                switch (type)
                {
                    case LogMessageType.Log:
                    {
                        logger.Info(message);
                        break;
                    }
                    case LogMessageType.Warning:
                    {
                        logger.Warn(message);
                        break;
                    }
                    case LogMessageType.Error:
                    {
                        logger.Error(message);
                        break;
                    }
                }
            }
        }

        public void Break()
        {
            // left blank intentionally
        }
    }
}
