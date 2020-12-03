using UserProfile.Server.Common;

namespace UserProfile.Service.Application.Components.Interfaces
{
    internal interface IDatabaseUserProfileCreator
    {
        void Create(int userId, int localId, ServerType serverType, ConnectionStatus connectionStatus);
    }
}