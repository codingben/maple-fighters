using System.Collections.Generic;

namespace GameServerProvider.Service.Application.Components.Interfaces
{
    internal interface IGameServersInformationProvider
    {
        IEnumerable<GameServerInformation> Provide();
        GameServerInformation? Provide(int id);
    }
}