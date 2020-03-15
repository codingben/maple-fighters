using Common.ComponentModel;
using CommonTools.Log;

namespace Authenticator.Domain.Aggregates.User.Services
{
    public class LoginService : ComponentBase, ILoginService
    {
        private IAccountRepository accountRepository;

        protected override void OnAwake()
        {
            base.OnAwake();

            accountRepository =
                Components.Get<IAccountRepository>().AssertNotNull();
        }

        public AuthenticationStatus Authenticate(
            string email,
            string password)
        {
            var authenticationStatus = AuthenticationStatus.NotFound;

            var account =
                accountRepository.Read(x => x.Email == email);
            if (account != null)
            {
                authenticationStatus =
                    account.Password == password
                        ? AuthenticationStatus.Authenticated
                        : AuthenticationStatus.WrongPassword;
            }

            return authenticationStatus;
        }
    }
}