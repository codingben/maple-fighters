namespace Login.Application.Components
{
    internal interface IDatabaseUserIdProvider
    {
        int GetUserId(string email);
    }
}