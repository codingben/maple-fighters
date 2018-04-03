namespace Login.Application.Components.Interfaces
{
    internal interface IDatabaseUserIdProvider
    {
        int GetUserId(string email);
    }
}