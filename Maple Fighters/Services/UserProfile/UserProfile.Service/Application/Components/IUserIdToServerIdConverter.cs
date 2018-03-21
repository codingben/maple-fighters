using System.Collections.Generic;
using ComponentModel.Common;

namespace UserProfile.Service.Application.Components
{
    internal interface IUserIdToServerIdConverter : IExposableComponent
    {
        void Add(int userId, int serverId);
        void Remove(int userId, int serverId);

        IEnumerable<int> Get(int userId);
    }
}