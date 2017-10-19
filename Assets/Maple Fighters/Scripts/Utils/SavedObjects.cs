using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scripts.Utils
{
    public static class SavedObjects
    {
        private static readonly List<GameObject> savedObjects = new List<GameObject>();

        public static void DontDestroyOnLoad(this GameObject obj)
        {
            Object.DontDestroyOnLoad(obj);

            savedObjects.Add(obj);
        }

        public static void Destroy(this GameObject obj)
        {
            Object.DestroyImmediate(obj);

            savedObjects.Remove(obj);
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
            savedObjects.Remove(obj);
        }

        public static IEnumerable<GameObject> GetSavedObjects()
        {
            return savedObjects;
        }
    }
}