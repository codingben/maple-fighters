using CommonTools.Log;
using Scripts.Utils;

namespace Scripts.Services
{
    public class LoggerInitializer : DontDestroyOnLoad<LoggerInitializer>
    {
        private void Awake()
        {
            LogUtils.Logger = new Logger();
        }
    }
}