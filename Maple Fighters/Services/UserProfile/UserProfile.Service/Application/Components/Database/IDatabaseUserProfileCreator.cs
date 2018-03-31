using ComponentModel.Common;
using UserProfile.Server.Common;

namespace UserProfile.Service.Application.Components
{
    internal interface IDatabaseUserProfileCreator : IExposableComponent
    {
        void Create(int userId, int localId, ServerType serverType, ConnectionStatus connectionStatus);
    }
}