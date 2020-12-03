namespace Registration.Application.Components.Interfaces
{
    internal interface IDatabaseUserCreator
    {
        void Create(string email, string password, string firstName, string lastName);
    }
}