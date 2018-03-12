using ComponentModel.Common;

namespace GameServerProvider.Service.Application.Components
{
    internal interface IGameServersInformationProvider : IExposableComponent
    {
        GameServerInformation[] Provide();
        GameServerInformation? Provide(int id);
    }
}