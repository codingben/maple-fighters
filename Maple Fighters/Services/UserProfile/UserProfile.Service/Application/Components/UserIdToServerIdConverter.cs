using System.Collections.Generic;
using System.Linq;
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
                if (HasServerId(userId, serverId))
                {
                    LogUtils.Log(MessageBuilder.Trace($"Unable to add a server with id {userId} to user id {userId} because it already exists."));
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

        public void Remove(int userId, int serverId)
        {
            lock (locker)
            {
                if (HasServerId(userId, serverId))
                {
                    var user = userIdsToServerIds[userId];
                    user.Remove(serverId);

                    if (user.Count == 0)
                    {
                        userIdsToServerIds.Remove(userId);
                    }
                }
                else
                {
                    LogUtils.Log(MessageBuilder.Trace($"Could not find a server id {userId} which assigned to a user id {userId}."));
                }
            }
        }

        public IEnumerable<int> Get(int userId)
        {
            lock (locker)
            {
                return userIdsToServerIds.TryGetValue(userId, out var serverIds) ? serverIds.ToArray() : null;
            }
        }

        public bool HasServerId(int userId, int serverId)
        {
            lock (locker)
            {
                var serverIds = Get(userId);
                return Get(userId) != null && serverIds.Any(id => serverId == id);
            }
        }
    }
}