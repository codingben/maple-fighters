namespace Login.Application.Components.Interfaces
{
    internal interface IDatabaseUserPasswordVerifier
    {
        bool Verify(string email, string password);
    }
}