namespace Authenticator.Domain.Aggregates.User
{
    public static class AccountFactory
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