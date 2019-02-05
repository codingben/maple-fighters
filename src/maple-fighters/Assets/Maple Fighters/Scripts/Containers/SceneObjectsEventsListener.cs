using Game.Common;
using UnityEngine;

namespace Scripts.Containers
{
    public class SceneObjectsEventsListener : MonoBehaviour
    {
        private void Awake()
        {
            var gameSceneApi = ServiceContainer.GameService.GetGameSceneApi();
            if (gameSceneApi != null)
            {
                // TODO: Add SceneLeft event.
                gameSceneApi.SceneEntered.AddListener(
                    OnSceneEntered);
                gameSceneApi.SceneObjectAdded.AddListener(
                    OnSceneObjectAdded);
                gameSceneApi.SceneObjectRemoved.AddListener(
                    OnSceneObjectRemoved);
                gameSceneApi.SceneObjectsAdded.AddListener(
                    OnSceneObjectsAdded);
                gameSceneApi.SceneObjectsRemoved.AddListener(
                    OnSceneObjectsRemoved);
            }
        }

        private void OnDestroy()
        {
            var gameSceneApi = ServiceContainer.GameService.GetGameSceneApi();
            if (gameSceneApi != null)
            {
                // TODO: Remove SceneLeft event.
                gameSceneApi.SceneEntered.RemoveListener(
                    OnSceneEntered);
                gameSceneApi.SceneObjectAdded.RemoveListener(
                    OnSceneObjectAdded);
                gameSceneApi.SceneObjectRemoved.RemoveListener(
                    OnSceneObjectRemoved);
                gameSceneApi.SceneObjectsAdded.RemoveListener(
                    OnSceneObjectsAdded);
                gameSceneApi.SceneObjectsRemoved.RemoveListener(
                    OnSceneObjectsRemoved);
            }
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