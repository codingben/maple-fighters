namespace Registration.Application.Components
{
    internal interface IDatabaseUserEmailVerifier
    {
        bool Verify(string email);
    }
}