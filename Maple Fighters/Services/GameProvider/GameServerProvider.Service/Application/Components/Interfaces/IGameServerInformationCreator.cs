namespace GameServerProvider.Service.Application.Components.Interfaces
{
    internal interface IGameServerInformationCreator
    {
        void Add(int id, GameServerInformation gameServerInformation);
    }
}