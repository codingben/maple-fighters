namespace Authenticator.Domain.Aggregates.User.Services
{
    public class RegistrationService : IRegistrationService
    {
        private readonly IAccountRepository accountRepository;

        public RegistrationService(IAccountRepository accountRepository)
        {
            this.accountRepository = accountRepository;
        }

        public AccountCreationStatus CreateAccount(Account account)
        {
            var email = accountRepository.Read(x => x.Email == account.Email);
            if (email != null)
            {
                return AccountCreationStatus.EmailExists;
            }
            else
            {
                accountRepository.Create(account);

                return AccountCreationStatus.Succeed;
            }
        }
    }
}