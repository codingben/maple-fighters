using ComponentModel.Common;

namespace UserProfile.Service.Application.Components
{
    internal interface IUserIdToServerIdConverter : IExposableComponent
    {
        void Add(int userId, int serverId);
        void Remove(int userId);

        int[] Get(int userId);
    }
}