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

            var isEmailExists =
                accountRepository.Read(x => x.Email == account.Email) != null;
            if (isEmailExists)
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