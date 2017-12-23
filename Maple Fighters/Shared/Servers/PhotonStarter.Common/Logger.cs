using System.IO;
using CommonTools.Log;
using log4net;
using log4net.Config;

namespace PhotonStarter.Common.Utils
{
    public class Logger : ILogger
    {
        private readonly ILog logger;
        private readonly object locker = new object();

        public Logger(string path)
        {
            logger = LogManager.GetLogger(typeof(Logger));

            var file = GetConfigurationFile(path);
            if (file == null)
            {
                return;
            }

            XmlConfigurator.ConfigureAndWatch(file);
        }

        public void Log(string message, LogMessageType type = LogMessageType.Log, object context = null)
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

        private static FileInfo GetConfigurationFile(string path)
        {
            var configurationFileInformation = new FileInfo(path);
            return !configurationFileInformation.Exists ? null : configurationFileInformation;
        }
    }
}