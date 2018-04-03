namespace UserProfile.Service.Application.Components.Interfaces
{
    internal interface IServerIdToPeerIdConverter
    {
        void Add(int serverId, int peerId);
        void Remove(int serverId);

        int? Get(int serverId);
    }
}