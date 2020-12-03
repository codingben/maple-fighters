using System.Collections.Generic;
using CommonTools.Log;
using ComponentModel.Common;

namespace UserProfile.Service.Application.PeerLogic.Components
{
    internal class UsersContainer : Component, IUsersContainer
    {
        private readonly List<int> userIds = new List<int>();

        public void Add(int userId)
        {
            if (userIds.Contains(userId))
            {
                LogUtils.Log(MessageBuilder.Trace($"A duplication of an authorized user with id {userId}"));
                return;
            }

            userIds.Add(userId);
        }

        public void Remove(int userId)
        {
            if (!userIds.Contains(userId))
            {
                LogUtils.Log(MessageBuilder.Trace($"Could not find an user with id {userId}"));
                return;
            }

            userIds.Remove(userId);
        }

        public IEnumerable<int> Get()
        {
            return userIds.ToArray();
        }
    }
}
