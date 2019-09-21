using System.Collections.Generic;
using Game.Common;
using UnityEngine;

namespace Scripts.World.Dummy
{
    public class DummyPortalContainer : MonoBehaviour
    {
        public static DummyPortalContainer GetInstance()
        {
            if (instance == null)
            {
                instance = FindObjectOfType<DummyPortalContainer>();
            }

            return instance;
        }

        private static DummyPortalContainer instance;
        private Dictionary<int, Maps> collection;

        private void Awake()
        {
            collection = new Dictionary<int, Maps>();
        }

        private void OnDestroy()
        {
            collection.Clear();
        }

        public void Add(int id, Maps map)
        {
            if (collection.ContainsKey(id))
            {
                Debug.LogWarning($"A portal with id {id} already exists.");
            }
            else
            {
                collection.Add(id, map);
            }
        }

        public void Remove(int id)
        {
            if (collection.ContainsKey(id))
            {
                collection.Remove(id);
            }
            else
            {
                Debug.LogWarning($"A portal with id {id} does not exist.");
            }
        }

        public Maps GetMap(int id)
        {
            if (!collection.TryGetValue(id, out var mapIndex))
            {
                Debug.LogWarning($"A portal with id {id} does not exist.");
            }

            return mapIndex;
        }
    }
}