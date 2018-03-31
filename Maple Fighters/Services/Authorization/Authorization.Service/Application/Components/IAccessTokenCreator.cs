using ComponentModel.Common;

namespace Authorization.Service.Application.Components
{
    internal interface IAccessTokenCreator : IExposableComponent
    {
        string Create(int userId);
    }
}