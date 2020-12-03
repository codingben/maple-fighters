namespace GameServerProvider.Service.Application.Components.Interfaces
{
    internal interface IGameServerInformationChanger
    {
        void Change(int id, GameServerInformation gameServerInformation);
    }
}