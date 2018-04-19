using System.Collections.Generic;
using CommonTools.Log;
using ComponentModel.Common;
using Game.Common;

namespace Scripts.Services
{
    public class PortalContainer : Component, IPortalContainer
    {
        private readonly Dictionary<int, Maps> portalIdToMapIndex = new Dictionary<int, Maps>();

        public void Add(int id, Maps map)
        {
            if (portalIdToMapIndex.ContainsKey(id))
            {
                LogUtils.Log($"A portal with id {id} already exists.");
                return;
            }

            portalIdToMapIndex.Add(id, map);
        }

        public void Remove(int id)
        {
            if (!portalIdToMapIndex.ContainsKey(id))
            {
                LogUtils.Log($"A portal with id {id} does not exist.");
                return;
            }

            portalIdToMapIndex.Remove(id);
        }

        public Maps GetMap(int id)
        {
            Maps mapIndex;

            var exists = portalIdToMapIndex.TryGetValue(id, out mapIndex);
            if (!exists)
            {
                LogUtils.Log($"A portal with id {id} does not exist.");
            }

            return mapIndex;
        }
    }
}