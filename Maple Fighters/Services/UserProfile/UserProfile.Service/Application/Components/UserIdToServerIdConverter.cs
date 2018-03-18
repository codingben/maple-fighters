using System.Collections.Generic;
using CommonTools.Log;
using ComponentModel.Common;

namespace UserProfile.Service.Application.Components
{
    internal class UserIdToServerIdConverter : Component, IUserIdToServerIdConverter
    {
        private readonly object locker = new object();
        private readonly Dictionary<int, List<int>> userIdsToServerIds = new Dictionary<int, List<int>>();

        public void Add(int userId, int serverId)
        {
            lock (locker)
            {
                if (userIdsToServerIds.ContainsKey(userId))
                {
                    LogUtils.Log(MessageBuilder.Trace($"Unable to add a user with id {serverId} because it already exists."));
                    return;
                }

                if (userIdsToServerIds.ContainsKey(userId))
                {
                    var user = userIdsToServerIds[userId];
                    user.Add(serverId);
                }
                else
                {
                    userIdsToServerIds.Add(userId, new List<int> { serverId });
                }
            }
        }

        public void Remove(int userId)
        {
            lock (locker)
            {
                if (!userIdsToServerIds.ContainsKey(userId))
                {
                    LogUtils.Log(MessageBuilder.Trace($"Could not find a user with id {userId}."));
                    return;
                }

                userIdsToServerIds.Remove(userId);
            }
        }

        public int[] Get(int userId)
        {
            lock (locker)
            {
                return userIdsToServerIds.TryGetValue(userId, out var serverIds) ? serverIds.ToArray() : null;
            }
        }
    }
}