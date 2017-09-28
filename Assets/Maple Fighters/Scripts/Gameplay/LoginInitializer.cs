using Scripts.Containers.Service;
using Scripts.Services;
using Scripts.Utils;

namespace Scripts.Gameplay
{
    public class LoginInitializer : DontDestroyOnLoad<GameInitializer>
    {
        private ILoginService loginService;
        private IRegistrationService registrationService;

        private void Awake()
        {
            loginService = ServiceContainer.LoginService;
            registrationService = ServiceContainer.RegistrationService;
        }

        private void Start()
        {
            loginService.Connect();
            registrationService.Connect();
        }

        private void OnApplicationQuit()
        {
            loginService.Disconnect();
            registrationService.Disconnect();
        }
    }
}