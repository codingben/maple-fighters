using CommonTools.Log;
using Game.Common;
using Scripts.Services;
using Scripts.Utils;

namespace Scripts.Containers
{
    public class SceneObjectsEventsListener : MonoSingleton<SceneObjectsEventsListener>
    {
        protected override void OnAwake()
        {
            base.OnAwake();

            SubscribeToEvents();
        }

        protected override void OnDestroying()
        {
            base.OnDestroying();

            UnsubscribeFromEvents();
        }

        private void SubscribeToEvents()
        {
            var gameScenePeerLogic = 
                ServiceContainer.GameService
                    .GetPeerLogic<IGameScenePeerLogicAPI>().AssertNotNull();

            // TODO: Add SceneLeft event.
            gameScenePeerLogic.SceneEntered.AddListener(OnSceneEntered);
            gameScenePeerLogic.SceneObjectAdded.AddListener(OnSceneObjectAdded);
            gameScenePeerLogic.SceneObjectRemoved.AddListener(OnSceneObjectRemoved);
            gameScenePeerLogic.SceneObjectsAdded.AddListener(OnSceneObjectsAdded);
            gameScenePeerLogic.SceneObjectsRemoved.AddListener(OnSceneObjectsRemoved);
        }

        private void UnsubscribeFromEvents()
        {
            var gameScenePeerLogic = 
                ServiceContainer.GameService
                    .GetPeerLogic<IGameScenePeerLogicAPI>().AssertNotNull();

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
            var sceneObjectsContainer = SceneObjectsContainer.GetInstance();
            if (sceneObjectsContainer != null)
            {
                var sceneObject = parameters.SceneObject;

                sceneObjectsContainer.SetLocalSceneObject(sceneObject.Id);
                sceneObjectsContainer.AddSceneObject(sceneObject);
            }
        }

        private void OnSceneObjectAdded(
            SceneObjectAddedEventParameters parameters)
        {
            var sceneObjectsContainer = SceneObjectsContainer.GetInstance();
            if (sceneObjectsContainer != null)
            {
                var sceneObject = parameters.SceneObject;
                sceneObjectsContainer.AddSceneObject(sceneObject);
            }
        }

        private void OnSceneObjectRemoved(
            SceneObjectRemovedEventParameters parameters)
        {
            var sceneObjectsContainer = SceneObjectsContainer.GetInstance();
            if (sceneObjectsContainer != null)
            {
                var sceneObject = parameters.SceneObjectId;
                sceneObjectsContainer.RemoveSceneObject(sceneObject);
            }
        }

        private void OnSceneObjectsAdded(
            SceneObjectsAddedEventParameters parameters)
        {
            var sceneObjects = parameters.SceneObjects;
            foreach (var sceneObject in sceneObjects)
            {
                var sceneObjectsContainer = SceneObjectsContainer.GetInstance();
                if (sceneObjectsContainer != null)
                {
                    sceneObjectsContainer.AddSceneObject(sceneObject);
                }
            }
        }

        private void OnSceneObjectsRemoved(
            SceneObjectsRemovedEventParameters parameters)
        {
            var sceneObjects = parameters.SceneObjectsId;
            foreach (var id in sceneObjects)
            {
                var sceneObjectsContainer = SceneObjectsContainer.GetInstance();
                if (sceneObjectsContainer != null)
                {
                    sceneObjectsContainer.RemoveSceneObject(id);
                }
            }
        }
    }
}