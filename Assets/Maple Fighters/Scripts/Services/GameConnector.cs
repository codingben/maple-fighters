using Scripts.Containers.Service;
using Scripts.Utils;

namespace Scripts.Services
{
    public class GameConnector : DontDestroyOnLoad<GameConnector>
    {
        private void Start()
        {
            ServiceContainer.GameService.Connect();
        }
    }
}