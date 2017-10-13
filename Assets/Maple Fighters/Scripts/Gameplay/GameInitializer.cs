using Scripts.Containers;
using Scripts.Services;
using Scripts.Utils;

namespace Scripts.Gameplay
{
    public class GameInitializer : DontDestroyOnLoad<GameInitializer>
    {
        private IGameService gameService;
        private IChatService chatService;

        private void Start()
        {
            gameService = ServiceContainer.GameService;
            chatService = ServiceContainer.ChatService;
        }

        private void OnApplicationQuit()
        {
            gameService.Disconnect();
            chatService.Disconnect();
        }
    }
}