using Scripts.Services;

namespace Scripts.Containers
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

        public static ILoginService LoginService
        {
            get
            {
                if (_loginService == null)
                {
                    _loginService = new LoginService();
                }
                return _loginService;
            }
        }

        public static IRegistrationService RegistrationService
        {
            get
            {
                if (_registrationService == null)
                {
                    _registrationService = new RegistrationService();
                }
                return _registrationService;
            }
        }

        private static IChatService _chatService;
        private static IGameService _gameService;
        private static ILoginService _loginService;
        private static IRegistrationService _registrationService;
    }
}