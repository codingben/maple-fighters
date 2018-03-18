using ComponentModel.Common;

namespace UserProfile.Service.Application.Components
{
    internal interface IServerIdToPeerIdConverter : IExposableComponent
    {
        void Add(int serverId, int peerId);
        void Remove(int serverId);

        int? Get(int serverId);
    }
}