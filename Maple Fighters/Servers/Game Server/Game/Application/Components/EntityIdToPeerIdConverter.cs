using System.Collections.Generic;
using CommonTools.Log;
using ServerApplication.Common.ComponentModel;

namespace Game.Application.Components
{
    internal class EntityIdToPeerIdConverter : IComponent
    {
        private readonly Dictionary<int, int> entityIdToPeerIdContainer = new Dictionary<int, int>();
        private readonly object locker = new object();

        public void AddEntityIdToPeerId(int entityId, int peerId)
        {
            lock (locker)
            {
                entityIdToPeerIdContainer.Add(entityId, peerId);
            }
        }

        public void RemoveEntityIdToPeerId(int entityId)
        {
            lock (locker)
            {
                entityIdToPeerIdContainer.Remove(entityId);
            }
        }

        public int GetPeerId(int entityId)
        {
            lock (locker)
            {
                if (entityIdToPeerIdContainer.TryGetValue(entityId, out var peerId))
                {
                    return peerId;
                }

                LogUtils.Log($"EntityIdToPeerIdConverter::GetPeerId() - Could not get a peer id of an entity id #{entityId}", LogMessageType.Warning);

                return -1;
            }
        }

        public void Dispose()
        {
            // Left blank intentionally
        }
    }
}