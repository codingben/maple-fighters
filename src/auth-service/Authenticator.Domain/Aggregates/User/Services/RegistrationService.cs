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
            AccountCreationStatus accountCreationStatus;

            var email = accountRepository.Read(x => x.Email == account.Email);
            if (email != null)
            {
                accountCreationStatus = AccountCreationStatus.EmailExists;
            }
            else
            {
                accountRepository.Create(account);

                accountCreationStatus = AccountCreationStatus.Succeed;
            }

            return accountCreationStatus;
        }
    }
}