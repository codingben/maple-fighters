using Common.ComponentModel;
using CommonTools.Log;

namespace Authenticator.Domain.Aggregates.User.Services
{
    public class RegistrationService : ComponentBase, IRegistrationService
    {
        private IAccountRepository accountRepository;

        protected override void OnAwake()
        {
            base.OnAwake();

            accountRepository =
                Components.Get<IAccountRepository>().AssertNotNull();
        }

        public AccountCreationStatus CreateAccount(
            Account account)
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