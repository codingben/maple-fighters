using System.Collections.Generic;
using Game.Common;
using UnityEngine;

namespace Scripts.World.Dummy
{
    public class DummyPortalContainer : MonoBehaviour
    {
        public static DummyPortalContainer GetInstance()
        {
            return instance;
        }

        private static DummyPortalContainer instance;
        private Dictionary<int, Maps> portalIdToMapMapper;

        private void Awake()
        {
            instance = this;
            portalIdToMapMapper = new Dictionary<int, Maps>();
        }

        private void OnDestroy()
        {
            portalIdToMapMapper.Clear();
        }

        public void Add(int id, Maps map)
        {
            if (portalIdToMapMapper.ContainsKey(id))
            {
                Debug.LogWarning($"A portal with id {id} already exists.");
            }
            else
            {
                portalIdToMapMapper.Add(id, map);
            }
        }

        public void Remove(int id)
        {
            if (portalIdToMapMapper.ContainsKey(id))
            {
                portalIdToMapMapper.Remove(id);
            }
            else
            {
                Debug.LogWarning($"A portal with id {id} does not exist.");
            }
        }

        public Maps GetMap(int id)
        {
            Maps mapIndex;

            if (!portalIdToMapMapper.TryGetValue(id, out mapIndex))
            {
                Debug.LogWarning($"A portal with id {id} does not exist.");
            }

            return mapIndex;
        }
    }
}