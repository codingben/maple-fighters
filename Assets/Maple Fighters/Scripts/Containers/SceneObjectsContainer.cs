using System.Collections.Generic;
using CommonTools.Log;
using Game.Common;
using Scripts.Gameplay;
using Scripts.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scripts.Containers
{
    public class SceneObjectsContainer : MonoSingleton<SceneObjectsContainer>
    {
        private Dictionary<int, ISceneObject> sceneObjects;
        private int localSceneObject;

        protected override void OnAwake()
        {
            base.OnAwake();

            sceneObjects = new Dictionary<int, ISceneObject>();

            // TODO: Remove it; Should be called from SceneLeft event!
            SceneManager.sceneLoaded += (x, y) => 
            {
                if (sceneObjects.ContainsKey(localSceneObject))
                {
                    sceneObjects.Remove(localSceneObject);
                }
            };
        }

        protected override void OnDestroying()
        {
            base.OnDestroying();

            sceneObjects.Clear();
        }

        public void SetLocalSceneObject(int id)
        {
            localSceneObject = id;
        }

        public void AddSceneObject(SceneObjectParameters parameters)
        {
            var id = parameters.Id;
            var name = parameters.Name;

            if (sceneObjects.ContainsKey(id))
            {
                LogUtils.Log(
                    MessageBuilder.Trace(
                        $"Scene object with id #{id} already exists."),
                    LogMessageType.Warning);
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

                    LogUtils.Log(
                        MessageBuilder.Trace(
                            $"Added a new scene object with id #{id}"));
                }
            }
        }

        public void RemoveSceneObject(int id)
        {
            var sceneObject = GetRemoteSceneObject(id)?.GameObject;
            if (sceneObject != null)
            {
                Destroy(sceneObject);

                sceneObjects.Remove(id);

                LogUtils.Log(
                    MessageBuilder.Trace(
                        $"Removed a scene object with id #{id}"));
            }
        }

        public void RemoveAllSceneObjects()
        {
            sceneObjects.Clear();
        }

        public ISceneObject GetLocalSceneObject()
        {
            ISceneObject sceneObject;

            if (!sceneObjects.TryGetValue(localSceneObject, out sceneObject))
            {
                LogUtils.Log(
                    MessageBuilder.Trace(
                        $"Could not find a local scene object with id #{localSceneObject}"),
                    LogMessageType.Warning);
            }

            return sceneObject;
        }

        public ISceneObject GetRemoteSceneObject(int id)
        {
            ISceneObject sceneObject;

            if (!sceneObjects.TryGetValue(id, out sceneObject))
            {
                LogUtils.Log(
                    MessageBuilder.Trace(
                        $"Could not find a scene object with id #{id}"),
                    LogMessageType.Warning);
            }

            return sceneObject;
        }

        private GameObject CreateGameObject(string name, Vector3 position)
        {
            GameObject gameObject = null;

            var sceneObject = Resources.Load($"Game/{name}")
                .AssertNotNull($"Could not find {name} scene object.");
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
                    LogUtils.Log(
                        MessageBuilder.Trace(
                            $"Could not create a scene object with name {name}"),
                        LogMessageType.Error);
                }
            }
            else
            {
                LogUtils.Log(
                    MessageBuilder.Trace(
                        $"Could not find a scene object with name {name}"),
                    LogMessageType.Error);
            }

            return gameObject;
        }
    }
}