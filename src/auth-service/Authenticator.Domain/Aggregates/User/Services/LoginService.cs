namespace Authenticator.Domain.Aggregates.User.Services
{
    public class LoginService : ILoginService
    {
        private readonly IAccountRepository accountRepository;

        public LoginService(IAccountRepository accountRepository)
        {
            this.accountRepository = accountRepository;
        }

        public Account FindAccount(string email)
        {
            return accountRepository.Read(x => x.Email == email);
        }
    }
}