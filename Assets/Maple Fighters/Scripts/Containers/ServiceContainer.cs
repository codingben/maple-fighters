using Scripts.Services;

namespace Scripts.Containers
{
    public static class ServiceContainer
    {
        public static IGameServiceAPI GameService
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

        public static ICharacterServiceAPI CharacterService
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

        public static IChatServiceAPI ChatService
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

        public static ILoginServiceAPI LoginService
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

        public static IRegistrationServiceAPI RegistrationService
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

        private static IGameServiceAPI gameService;
        private static ICharacterServiceAPI characterService;
        private static IChatServiceAPI chatService;
        private static ILoginServiceAPI loginService;
        private static IRegistrationServiceAPI registrationService;
    }
}