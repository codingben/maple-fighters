namespace Authenticator.Domain.Aggregates.User.Services
{
    public class LoginService : ILoginService
    {
        private readonly IAccountRepository accountRepository;

        public LoginService(IAccountRepository accountRepository)
        {
            this.accountRepository = accountRepository;
        }

        public IAccount FindAccount(string email)
        {
            return accountRepository.Read(x => x.Email == email);
        }
    }
}