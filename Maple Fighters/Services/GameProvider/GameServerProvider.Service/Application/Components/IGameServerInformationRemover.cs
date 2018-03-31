using ComponentModel.Common;

namespace GameServerProvider.Service.Application.Components
{
    internal interface IGameServerInformationRemover : IExposableComponent
    {
        void Remove(int id);
    }
}