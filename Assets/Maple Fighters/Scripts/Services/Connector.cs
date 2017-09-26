using Scripts.Containers.Service;
using Scripts.Utils;

namespace Scripts.Services
{
    public class Connector : DontDestroyOnLoad<Connector>
    {
        private void Start()
        {
            ServiceContainer.GameService.Connect();
            ServiceContainer.ChatService.Connect();
        }
    }
}