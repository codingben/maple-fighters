using ComponentModel.Common;

namespace Authorization.Service.Application.Components
{
    internal interface IAccessTokenRemover : IExposableComponent
    {
        void Remove(int userId);
    }
}