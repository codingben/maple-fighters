using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Utils
{
    public static class LoadedObjects
    {
        private static readonly List<GameObject> loadedObjects = new List<GameObject>();

        public static void DontDestroyOnLoad(this GameObject obj)
        {
            loadedObjects.Add(obj);
            Object.DontDestroyOnLoad(obj);
        }

        public static void Destroy(this GameObject obj)
        {
            loadedObjects.Remove(obj);
            Object.DestroyImmediate(obj);
        }

        public static void DestroyAll()
        {
            var objects = new List<GameObject>();
            objects.AddRange(GetLoadedObjects());

            foreach (var obj in objects)
            {
                obj.Destroy();
            }
        }

        public static IEnumerable<GameObject> GetLoadedObjects()
        {
            return loadedObjects;
        }
    }
}