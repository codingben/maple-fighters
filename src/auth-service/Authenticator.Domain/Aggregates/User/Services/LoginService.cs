namespace Authenticator.Domain.Aggregates.User.Services
{
    public class LoginService : ILoginService
    {
        private readonly IAccountRepository accountRepository;

        public LoginService(IAccountRepository accountRepository)
        {
            this.accountRepository = accountRepository;
        }

        public Account Authenticate(string email, string password)
        {
            var account =
                accountRepository.Read(x => x.Email == email);
            if (account != null)
            {
                if (account.Password == password)
                {
                    return account;
                }
            }

            return null;
        }
    }
}