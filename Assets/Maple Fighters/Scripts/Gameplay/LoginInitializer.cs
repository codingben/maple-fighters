using Scripts.Containers.Service;
using Scripts.Services;
using UnityEngine;

namespace Scripts.Gameplay
{
    public class LoginInitializer : MonoBehaviour
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

        private void OnDestroy()
        {
            loginService.Disconnect();
            registrationService.Disconnect();
        }

        private void OnApplicationQuit()
        {
            loginService.Disconnect();
            registrationService.Disconnect();
        }
    }
}