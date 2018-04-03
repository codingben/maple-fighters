namespace Login.Application.Components.Interfaces
{
    internal interface IDatabaseUserVerifier
    {
        bool IsExists(string email);
    }
}