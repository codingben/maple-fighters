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

        private static IGameService _gameService;
    }
}