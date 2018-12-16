using CommonTools.Log;
using Scripts.Utils;

namespace Scripts.Services
{
    public class LogUtilsCreator : MonoSingleton<LogUtilsCreator>
    {
        protected override void OnAwake()
        {
            base.OnAwake();

            LogUtils.Logger = new Logger();

            Destroy(gameObject);
        }
    }
}