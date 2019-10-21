using System;
using Network.Utils;

namespace Scripts.Services.Authenticator
{
    public class AuthenticatorService : Singleton<AuthenticatorService>, IAuthenticatorService
    {
        public IAuthenticatorApi AuthenticatorApi { get; set; }

        private void OnDestroy()
        {
            (AuthenticatorApi as IDisposable)?.Dispose();
        }
    }
}