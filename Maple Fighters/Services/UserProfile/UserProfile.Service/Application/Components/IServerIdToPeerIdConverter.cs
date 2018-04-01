namespace UserProfile.Service.Application.Components
{
    internal interface IServerIdToPeerIdConverter
    {
        void Add(int serverId, int peerId);
        void Remove(int serverId);

        int? Get(int serverId);
    }
}