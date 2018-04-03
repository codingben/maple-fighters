using System.Collections.Generic;

namespace UserProfile.Service.Application.PeerLogic.Components
{
    internal interface IUsersContainer
    {
        void Add(int userId);
        void Remove(int userId);

        IEnumerable<int> Get();
    }
}