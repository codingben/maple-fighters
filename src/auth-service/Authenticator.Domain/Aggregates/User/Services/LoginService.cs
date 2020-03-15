namespace Authenticator.Domain.Aggregates.User.Services
{
    public class LoginService : ILoginService
    {
        private readonly IAccountRepository accountRepository;

        public LoginService(IAccountRepository accountRepository)
        {
            this.accountRepository = accountRepository;
        }

        public AuthenticationStatus Authenticate(string email, string password)
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