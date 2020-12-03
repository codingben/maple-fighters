using System.Collections.Generic;

namespace UserProfile.Service.Application.Components.Interfaces
{
    internal interface IUserIdToServerIdConverter
    {
        void Add(int userId, int serverId);
        void Remove(int userId, int serverId);

        IEnumerable<int> Get(int userId);
    }
}