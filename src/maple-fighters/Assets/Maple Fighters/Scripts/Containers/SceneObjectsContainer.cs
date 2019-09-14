using System.Collections.Generic;
using Game.Common;
using Scripts.Gameplay;
using UnityEngine;

namespace Scripts.Containers
{
    public class SceneObjectsContainer : MonoBehaviour, ISceneObjectsContainer
    {
        public static SceneObjectsContainer GetInstance()
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SceneObjectsContainer>();
            }

            return instance;
        }

        private static SceneObjectsContainer instance;

        private ISceneObject localSceneObject;
        private Dictionary<int, ISceneObject> sceneObjects;

        public void SetLocalSceneObject(ISceneObject sceneObject)
        {
            localSceneObject = sceneObject;
        }

        private void Awake()
        {
            sceneObjects = new Dictionary<int, ISceneObject>();
        }

        private void OnDestroy()
        {
            sceneObjects.Clear();
        }

        public ISceneObject AddSceneObject(SceneObjectParameters parameters)
        {
            var id = parameters.Id;
            var name = parameters.Name;

            if (sceneObjects.ContainsKey(id))
            {
                Debug.LogWarning($"Scene object with id #{id} already exists.");
            }
            else
            {
                var gameObject = CreateGameObject(
                    name,
                    new Vector3(parameters.X, parameters.Y));
                if (gameObject != null)
                {
                    var sceneObject = gameObject.GetComponent<ISceneObject>();
                    sceneObject.Id = id;

                    sceneObjects.Add(id, sceneObject);

                    Debug.Log($"Added a new scene object with id #{id}");
                }
            }

            return sceneObjects[id];
        }

        public void RemoveSceneObject(int id)
        {
            var sceneObject = GetRemoteSceneObject(id)?.GameObject;
            if (sceneObject != null)
            {
                sceneObjects.Remove(id);

                Destroy(sceneObject);

                Debug.Log($"Removed a scene object with id #{id}");
            }
        }

        public ISceneObject GetLocalSceneObject()
        {
            return localSceneObject;
        }

        public ISceneObject GetRemoteSceneObject(int id)
        {
            ISceneObject sceneObject;

            if (!sceneObjects.TryGetValue(id, out sceneObject))
            {
                Debug.LogWarning(
                    $"Could not find a scene object with id #{id}");
            }

            return sceneObject;
        }

        private GameObject CreateGameObject(string name, Vector3 position)
        {
            GameObject gameObject = null;

            var sceneObject = Resources.Load($"Game/{name}");
            if (sceneObject != null)
            {
                gameObject = 
                    Instantiate(sceneObject, position, Quaternion.identity) 
                        as GameObject;
                if (gameObject != null)
                {
                    gameObject.name = name;
                }
                else
                {
                    Debug.LogError(
                        $"Could not create a scene object with name {name}");
                }
            }
            else
            {
                Debug.LogError(
                    $"Could not find a scene object with name {name}");
            }

            return gameObject;
        }
    }
}