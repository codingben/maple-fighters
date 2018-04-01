namespace Login.Application.Components
{
    internal interface IDatabaseUserPasswordVerifier
    {
        bool Verify(string email, string password);
    }
}