using System.Collections.Generic;
using ComponentModel.Common;

namespace UserProfile.Service.Application.PeerLogic.Components
{
    internal interface IUsersContainer : IExposableComponent
    {
        void Add(int userId);
        void Remove(int userId);

        IEnumerable<int> Get();
    }
}