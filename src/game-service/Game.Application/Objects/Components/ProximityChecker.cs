using System.Collections.Generic;
using Common.ComponentModel;
using InterestManagement;

namespace Game.Application.Objects.Components
{
    [ComponentSettings(ExposedState.Exposable)]
    public class ProximityChecker : ComponentBase, IProximityChecker
    {
        private IInterestArea<IGameObject> interestArea;
        private List<IGameObject> nearbyGameObjects;

        public ProximityChecker()
        {
            nearbyGameObjects = new List<IGameObject>();
        }

        protected override void OnAwake()
        {
            var presenceSceneProvider = Components.Get<IPresenceSceneProvider>();
            var scene = presenceSceneProvider.GetScene();
            var gameObjectGetter = Components.Get<IGameObjectGetter>();
            var gameObject = gameObjectGetter.Get();

            interestArea = new InterestArea<IGameObject>(scene, gameObject);

            SubscribeToNearbySceneObjectsEvents();
        }

        protected override void OnRemoved()
        {
            UnsubscribeFromNearbySceneObjectsEvents();

            interestArea?.Dispose();
        }

        private void SubscribeToNearbySceneObjectsEvents()
        {
            interestArea.NearbySceneObjectsEvents.SceneObjectAdded += OnGameObjectAdded;
            interestArea.NearbySceneObjectsEvents.SceneObjectRemoved += GameObjectRemoved;
            interestArea.NearbySceneObjectsEvents.SceneObjectsAdded += OnGameObjectsAdded;
            interestArea.NearbySceneObjectsEvents.SceneObjectsRemoved += OnGameObjectsRemoved;
        }

        private void UnsubscribeFromNearbySceneObjectsEvents()
        {
            interestArea.NearbySceneObjectsEvents.SceneObjectAdded -= OnGameObjectAdded;
            interestArea.NearbySceneObjectsEvents.SceneObjectRemoved -= GameObjectRemoved;
            interestArea.NearbySceneObjectsEvents.SceneObjectsAdded -= OnGameObjectsAdded;
            interestArea.NearbySceneObjectsEvents.SceneObjectsRemoved -= OnGameObjectsRemoved;
        }

        private void OnGameObjectAdded(IGameObject gameObject)
        {
            nearbyGameObjects.Add(gameObject);
        }

        private void GameObjectRemoved(IGameObject gameObject)
        {
            nearbyGameObjects.Remove(gameObject);
        }

        private void OnGameObjectsAdded(IEnumerable<IGameObject> gameObjects)
        {
            nearbyGameObjects.AddRange(gameObjects);
        }

        private void OnGameObjectsRemoved(IEnumerable<IGameObject> gameObjects)
        {
            foreach (var gameObject in gameObjects)
            {
                nearbyGameObjects.Remove(gameObject);
            }
        }

        public IEnumerable<IGameObject> GetNearbyGameObjects()
        {
            return nearbyGameObjects;
        }

        public INearbySceneObjectsEvents<IGameObject> GetNearbyGameObjectsEvents()
        {
            return interestArea.NearbySceneObjectsEvents;
        }
    }
}