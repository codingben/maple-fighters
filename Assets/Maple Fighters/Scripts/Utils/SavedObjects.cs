using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Utils
{
    public static class SavedObjects
    {
        private static readonly HashSet<GameObject> GameObjects;

        static SavedObjects()
        {
            GameObjects = new HashSet<GameObject>();
        }

        public static void DontDestroyOnLoad(GameObject gameObject)
        {
            GameObjects.Add(gameObject);

            Object.DontDestroyOnLoad(gameObject);
        }

        public static void Destroy(GameObject gameObject)
        {
            GameObjects.Remove(gameObject);

            Object.Destroy(gameObject);
        }

        public static void DestroyAll()
        {
            var gameObjects = new HashSet<GameObject>(GameObjects);
            foreach (var gameObject in gameObjects)
            {
                Destroy(gameObject);
            }
        }
    }
}