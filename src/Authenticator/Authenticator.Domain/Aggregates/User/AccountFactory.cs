namespace Authenticator.Domain.Aggregates.User
{
    public class AccountFactory
    {
        public static Account CreateAccount(
            string email,
            string password,
            string firstName,
            string lastName)
        {
            return new Account(email, password, firstName, lastName);
        }
    }
}