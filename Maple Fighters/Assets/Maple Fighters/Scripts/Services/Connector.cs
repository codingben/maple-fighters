using CommonTools.Log;
using Scripts.Containers.Service;
using Scripts.Utils;

namespace Scripts.Services
{
    public class Connector : DontDestroyOnLoad<Connector>
    {
        private void Start()
        {
            LogUtils.Log(MessageBuilder.Trace());
            ServiceContainer.GameService.Connect();
        }
    }
}