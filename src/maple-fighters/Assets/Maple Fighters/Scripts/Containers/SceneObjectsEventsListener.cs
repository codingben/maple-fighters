using Game.Common;
using Scripts.Services;
using UnityEngine;

namespace Scripts.Containers
{
    public class SceneObjectsEventsListener : MonoBehaviour
    {
        private void Awake()
        {
            var gameScenePeerLogic = ServiceContainer.GameService
                .GetPeerLogic<IGameScenePeerLogicAPI>();

            // TODO: Add SceneLeft event.
            gameScenePeerLogic.SceneEntered.AddListener(OnSceneEntered);
            gameScenePeerLogic.SceneObjectAdded.AddListener(OnSceneObjectAdded);
            gameScenePeerLogic.SceneObjectRemoved.AddListener(OnSceneObjectRemoved);
            gameScenePeerLogic.SceneObjectsAdded.AddListener(OnSceneObjectsAdded);
            gameScenePeerLogic.SceneObjectsRemoved.AddListener(OnSceneObjectsRemoved);
        }

        private void OnDestroy()
        {
            var gameScenePeerLogic = ServiceContainer.GameService
                .GetPeerLogic<IGameScenePeerLogicAPI>();

            // TODO: Remove SceneLeft event.
            gameScenePeerLogic.SceneEntered.RemoveListener(OnSceneEntered);
            gameScenePeerLogic.SceneObjectAdded.RemoveListener(OnSceneObjectAdded);
            gameScenePeerLogic.SceneObjectRemoved.RemoveListener(OnSceneObjectRemoved);
            gameScenePeerLogic.SceneObjectsAdded.RemoveListener(OnSceneObjectsAdded);
            gameScenePeerLogic.SceneObjectsRemoved.RemoveListener(OnSceneObjectsRemoved);
        }

        private void OnSceneEntered(
            EnterSceneResponseParameters parameters)
        {
            var localSceneObject = 
                SceneObjectsContainer.GetInstance()
                    .AddSceneObject(parameters.SceneObject);
            SceneObjectsContainer.GetInstance().SetLocalSceneObject(
                localSceneObject);
        }

        private void OnSceneObjectAdded(
            SceneObjectAddedEventParameters parameters)
        {
            SceneObjectsContainer.GetInstance().AddSceneObject(
                parameters.SceneObject);
        }

        private void OnSceneObjectRemoved(
            SceneObjectRemovedEventParameters parameters)
        {
            SceneObjectsContainer.GetInstance().RemoveSceneObject(
                parameters.SceneObjectId);
        }

        private void OnSceneObjectsAdded(
            SceneObjectsAddedEventParameters parameters)
        {
            foreach (var sceneObject in parameters.SceneObjects)
            {
                SceneObjectsContainer.GetInstance().AddSceneObject(sceneObject);
            }
        }

        private void OnSceneObjectsRemoved(
            SceneObjectsRemovedEventParameters parameters)
        {
            foreach (var id in parameters.SceneObjectsId)
            {
                SceneObjectsContainer.GetInstance().RemoveSceneObject(id);
            }
        }
    }
}