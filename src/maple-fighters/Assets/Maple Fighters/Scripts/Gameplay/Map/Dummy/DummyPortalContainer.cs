using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Gameplay.Map.Dummy
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
        private Dictionary<int, DummyMaps> collection;

        private void Awake()
        {
            // TODO: Refactor this
            collection = new Dictionary<int, DummyMaps>
            {
                { 2, DummyMaps.Map_2 },
                { 3, DummyMaps.Map_1 }
            };
        }

        private void OnDestroy()
        {
            collection?.Clear();
        }

        public DummyMaps GetMap(int id)
        {
            if (collection.TryGetValue(id, out var mapIndex) == false)
            {
                Debug.LogWarning($"A portal with id {id} does not exist.");
            }

            return mapIndex;
        }
    }
}