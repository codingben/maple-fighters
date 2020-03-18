namespace Authenticator.API.Datas
{
    public struct RegistrationData
    {
        public string Email { get; }

        public string Password { get; }

        public string FirstName { get; }

        public string LastName { get; }

        public RegistrationData(
            string email,
            string password,
            string firstName,
            string lastName)
        {
            Email = email;
            Password = password;
            FirstName = firstName;
            LastName = lastName;
        }
    }
}