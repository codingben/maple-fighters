using ComponentModel.Common;
using UserProfile.Server.Common;

namespace UserProfile.Service.Application.Components
{
    internal interface IDatabaseUserProfilePropertiesUpdater : IExposableComponent
    {
        void Update(int userId, int localId, ServerType serverType, ConnectionStatus connectionStatus);
    }
}