namespace Authorization.Service.Application.Components.Interfaces
{
    internal interface IAccessTokenRemover
    {
        void Remove(int userId);
    }
}