using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Gameplay.Map.Dummy
{
    // TODO: Move out
    public enum Maps
    {
        Map_1 = 2,
        Map_2 = 3
    }

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
            // TODO: Refactor this
            collection = new Dictionary<int, Maps>
            {
                { 2, Maps.Map_2 },
                { 3, Maps.Map_1 }
            };
        }

        private void OnDestroy()
        {
            collection?.Clear();
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