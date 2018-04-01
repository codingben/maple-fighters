namespace Authorization.Service.Application.Components
{
    internal interface IAccessTokenRemover
    {
        void Remove(int userId);
    }
}