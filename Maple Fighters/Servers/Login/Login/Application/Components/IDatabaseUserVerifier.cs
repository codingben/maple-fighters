namespace Login.Application.Components
{
    internal interface IDatabaseUserVerifier
    {
        bool IsExists(string email);
    }
}