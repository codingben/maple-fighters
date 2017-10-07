using Scripts.Containers.Service;
using Scripts.Utils;

namespace Scripts.Services
{
    public class ChatConnector : DontDestroyOnLoad<ChatConnector>
    {
        private void Start()
        {
            ServiceContainer.ChatService.Connect();
        }
    }
}