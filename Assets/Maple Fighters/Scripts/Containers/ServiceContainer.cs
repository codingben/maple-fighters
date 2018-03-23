using Scripts.Services;

namespace Scripts.Containers
{
    public static class ServiceContainer
    {
        public static IServiceBase AuthorizationService
        {
            get
            {
                if (authorizationService == null)
                {
                    authorizationService = new ServiceBase();
                }
                return authorizationService;
            }
        }

        public static IServiceBase GameServerProviderService
        {
            get
            {
                if (gameServerProviderService == null)
                {
                    gameServerProviderService = new ServiceBase();
                }
                return gameServerProviderService;
            }
        }

        public static IServiceBase GameService
        {
            get
            {
                if (gameService == null)
                {
                    gameService = new ServiceBase();
                }
                return gameService;
            }
        }

        public static IServiceBase CharacterService
        {
            get
            {
                if (characterService == null)
                {
                    characterService = new ServiceBase();
                }
                return characterService;
            }
        }

        public static IServiceBase ChatService
        {
            get
            {
                if (chatService == null)
                {
                    chatService = new ServiceBase();
                }
                return chatService;
            }
        }

        public static IServiceBase LoginService
        {
            get
            {
                if (loginService == null)
                {
                    loginService = new ServiceBase();
                }
                return loginService;
            }
        }

        public static IServiceBase RegistrationService
        {
            get
            {
                if (registrationService == null)
                {
                    registrationService = new ServiceBase();
                }
                return registrationService;
            }
        }

        private static IServiceBase authorizationService;
        private static IServiceBase gameServerProviderService;
        private static IServiceBase gameService;
        private static IServiceBase characterService;
        private static IServiceBase chatService;
        private static IServiceBase loginService;
        private static IServiceBase registrationService;
    }
}