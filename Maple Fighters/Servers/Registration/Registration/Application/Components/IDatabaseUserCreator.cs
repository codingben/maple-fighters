namespace Registration.Application.Components
{
    internal interface IDatabaseUserCreator
    {
        void Create(string email, string password, string firstName, string lastName);
    }
}