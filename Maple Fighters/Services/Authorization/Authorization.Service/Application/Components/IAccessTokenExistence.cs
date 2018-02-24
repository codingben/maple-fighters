using ComponentModel.Common;

namespace Authorization.Service.Application.Components
{
    internal interface IAccessTokenExistence : IExposableComponent
    {
        bool Exists(int userId);
        bool Exists(string accessToken);
    }
}