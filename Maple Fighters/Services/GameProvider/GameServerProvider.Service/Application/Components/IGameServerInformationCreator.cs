using ComponentModel.Common;

namespace GameServerProvider.Service.Application.Components
{
    internal interface IGameServerInformationCreator : IExposableComponent
    {
        void Add(int id, GameServerInformation gameServerInformation);
    }
}