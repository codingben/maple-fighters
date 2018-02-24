using ComponentModel.Common;

namespace Authorization.Service.Application.Components
{
    internal interface IAccessTokenCreator : IExposableComponent
    {
        void Create(int userId);
    }
}