using CommonTools.Coroutines;
using CommonTools.Log;
using Scripts.Utils;

namespace Scripts.Services
{
    public class ServiceInitializer : DontDestroyOnLoad<ServiceInitializer>
    {
        protected override void OnAwake()
        {
            base.OnAwake();

            LogUtils.Logger = new Logger();
            TimeProviders.DefaultTimeProvider = new GameTimeProvider();
        }
    }
}