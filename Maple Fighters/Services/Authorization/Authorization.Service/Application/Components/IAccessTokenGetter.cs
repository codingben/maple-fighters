using ComponentModel.Common;

namespace Authorization.Service.Application.Components
{
    internal interface IAccessTokenGetter : IExposableComponent
    {
        string Get(int userId);
    }
}