using Scripts.Services;

namespace Scripts.Containers.Service
{
    public static class ServiceContainer
    {
        public static IGameService GameService
        {
            get
            {
                if (_gameService == null)
                {
                    _gameService = new GameService();
                }

                return _gameService;
            }
        }

        public static IChatService ChatService
        {
            get
            {
                if (_chatService == null)
                {
                    _chatService = new ChatService();
                }

                return _chatService;
            }
        }

        private static IChatService _chatService;
        private static IGameService _gameService;
    }
}