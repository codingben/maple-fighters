namespace GameServerProvider.Service.Application.Components
{
    internal interface IGameServerInformationCreator
    {
        void Add(int id, GameServerInformation gameServerInformation);
    }
}