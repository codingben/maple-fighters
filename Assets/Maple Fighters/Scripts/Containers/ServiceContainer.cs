using Scripts.Services;

namespace Scripts.Containers
{
    public static class ServiceContainer
    {
        public static IGameService GameService
        {
            get
            {
                if (gameService == null)
                {
                    gameService = new GameService();
                }
                return gameService;
            }
        }

        public static ICharacterService CharacterService
        {
            get
            {
                if (characterService == null)
                {
                    characterService = new CharacterService();
                }
                return characterService;
            }
        }

        public static IChatService ChatService
        {
            get
            {
                if (chatService == null)
                {
                    chatService = new ChatService();
                }
                return chatService;
            }
        }

        public static ILoginService LoginService
        {
            get
            {
                if (loginService == null)
                {
                    loginService = new LoginService();
                }
                return loginService;
            }
        }

        public static IRegistrationService RegistrationService
        {
            get
            {
                if (registrationService == null)
                {
                    registrationService = new RegistrationService();
                }
                return registrationService;
            }
        }

        private static IGameService gameService;
        private static ICharacterService characterService;
        private static IChatService chatService;
        private static ILoginService loginService;
        private static IRegistrationService registrationService;
    }
}