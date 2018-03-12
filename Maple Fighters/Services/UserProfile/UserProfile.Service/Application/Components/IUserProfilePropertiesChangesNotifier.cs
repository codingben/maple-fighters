using ComponentModel.Common;
using UserProfile.Server.Common;

namespace UserProfile.Service.Application.Components
{
    internal interface IUserProfilePropertiesChangesNotifier : IExposableComponent
    {
        void Notify(ServerType serverType, ConnectionStatus connectionStatus);
    }
}