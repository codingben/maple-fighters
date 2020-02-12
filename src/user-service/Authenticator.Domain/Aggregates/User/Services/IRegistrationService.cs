namespace Authenticator.Domain.Aggregates.User.Services
{
    public interface IRegistrationService
    {
        AccountCreationStatus CreateAccount(Account account);
    }
}