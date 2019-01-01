using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Utils
{
    public class SavedGameObjects : Singleton<SavedGameObjects>
    {
        private readonly HashSet<GameObject> savedGameObjects;

        public SavedGameObjects()
        {
            savedGameObjects = new HashSet<GameObject>();
        }

        public void DontDestroyOnLoad(GameObject gameObject)
        {
            savedGameObjects.Add(gameObject);

            Object.DontDestroyOnLoad(gameObject);
        }

        public void Destroy(GameObject gameObject)
        {
            savedGameObjects.Remove(gameObject);

            Object.Destroy(gameObject);
        }

        public void DestroyAll()
        {
            var gameObjects = new HashSet<GameObject>(savedGameObjects);
            foreach (var gameObject in gameObjects)
            {
                Destroy(gameObject);
            }
        }
    }
}