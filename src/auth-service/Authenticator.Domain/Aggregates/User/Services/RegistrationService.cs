namespace Authenticator.Domain.Aggregates.User.Services
{
    public class RegistrationService : IRegistrationService
    {
        private readonly IAccountRepository accountRepository;

        public RegistrationService(IAccountRepository accountRepository)
        {
            this.accountRepository = accountRepository;
        }

        public void CreateAccount(Account account)
        {
            accountRepository.Create(account);
        }

        public bool VerifyEmail(string email)
        {
            return accountRepository.Read(x => x.Email == email) == null;
        }
    }
}