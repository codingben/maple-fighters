using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

        private static void Destroy(this GameObject obj)
        {
            loadedObjects.Remove(obj);
            Object.DestroyImmediate(obj);
        }

        public static void DestroyAll()
        {
            var objects = new List<GameObject>();
            objects.AddRange(GetSavedObjects());

            foreach (var obj in objects)
            {
                obj.Destroy();
            }

            SceneManager.LoadScene(0, LoadSceneMode.Single);
        }

        public static void RemoveSavedObjectOnly(this GameObject obj)
        {
            loadedObjects.Remove(obj);
        }

        public static IEnumerable<GameObject> GetSavedObjects()
        {
            return loadedObjects;
        }
    }
}