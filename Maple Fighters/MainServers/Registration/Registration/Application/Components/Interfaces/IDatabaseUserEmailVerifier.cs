namespace Registration.Application.Components.Interfaces
{
    internal interface IDatabaseUserEmailVerifier
    {
        bool Verify(string email);
    }
}