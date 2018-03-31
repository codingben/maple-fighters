using System.Collections.Generic;
using CommonTools.Log;
using ComponentModel.Common;

namespace UserProfile.Service.Application.Components
{
    internal class ServerIdToPeerIdConverter : Component, IServerIdToPeerIdConverter
    {
        private readonly object locker = new object();
        private readonly Dictionary<int, int> serverIdsToPeerIds = new Dictionary<int, int>();

        public void Add(int serverId, int peerId)
        {
            lock (locker)
            {
                if (serverIdsToPeerIds.ContainsKey(serverId))
                {
                    LogUtils.Log(MessageBuilder.Trace($"Unable to add a server with id {serverId} because it already exists."));
                    return;
                }

                serverIdsToPeerIds.Add(serverId, peerId);
            }
        }

        public void Remove(int serverId)
        {
            lock (locker)
            {
                if (!serverIdsToPeerIds.ContainsKey(serverId))
                {
                    LogUtils.Log(MessageBuilder.Trace($"Could not find a server with id {serverId}."));
                    return;
                }

                serverIdsToPeerIds.Remove(serverId);
            }
        }

        public int? Get(int serverId)
        {
            lock (locker)
            {
                if (serverIdsToPeerIds.TryGetValue(serverId, out var peerId))
                {
                    return peerId;
                }

                LogUtils.Log(MessageBuilder.Trace($"Could not find a server with id {serverId}."));
                return null;
            }
        }
    }
}