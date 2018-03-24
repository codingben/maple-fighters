using System.Collections.Generic;
using CommonTools.Log;
using ComponentModel.Common;

namespace UserProfile.Server.Common
{
    public class UserIdToPeerIdConverter : Component, IUserIdToPeerIdConverter
    {
        private readonly object locker = new object();
        private readonly Dictionary<int, int> userIdsToPeerIds = new Dictionary<int, int>();

        public void Add(int userId, int peerId)
        {
            lock (locker)
            {
                if (userIdsToPeerIds.ContainsKey(userId))
                {
                    LogUtils.Log(MessageBuilder.Trace($"Unable to add a user id {userId} because it already exists."));
                    return;
                }

                userIdsToPeerIds.Add(userId, peerId);
            }
        }

        public void Remove(int userId)
        {
            lock (locker)
            {
                if (!userIdsToPeerIds.ContainsKey(userId))
                {
                    LogUtils.Log(MessageBuilder.Trace($"Could not find a user with id {userId}."));
                    return;
                }

                userIdsToPeerIds.Remove(userId);
            }
        }

        public int? Get(int userId)
        {
            lock (locker)
            {
                if (userIdsToPeerIds.TryGetValue(userId, out var peerId))
                {
                    return peerId;
                }

                LogUtils.Log(MessageBuilder.Trace($"Could not find a user with id {userId}."));
                return null;
            }
        }
    }
}