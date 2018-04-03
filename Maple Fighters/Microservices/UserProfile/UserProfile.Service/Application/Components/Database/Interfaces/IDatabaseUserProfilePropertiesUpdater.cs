using UserProfile.Server.Common;

namespace UserProfile.Service.Application.Components.Interfaces
{
    internal interface IDatabaseUserProfilePropertiesUpdater
    {
        void Update(int userId, int localId, ServerType serverType, ConnectionStatus connectionStatus);
    }
}