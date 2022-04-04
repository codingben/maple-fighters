namespace Authenticator.Domain.Aggregates.User.Services
{
    public interface IRegistrationService
    {
        void CreateAccount(IAccount account);

        bool CheckIfEmailExists(string email);
    }
}