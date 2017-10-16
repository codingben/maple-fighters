using CommonTools.Coroutines;
using CommonTools.Log;
using Scripts.Utils;

namespace Scripts.Services
{
    public class ServiceInitializer : DontDestroyOnLoad<ServiceInitializer>
    {
        private static bool initialized;

        private void Awake() // It may be called a couple times
        {
            if (initialized)
            {
                return;
            }

            LogUtils.Logger = new Logger();
            TimeProviders.DefaultTimeProvider = new GameTimeProvider();

            initialized = true;
        }
    }
}