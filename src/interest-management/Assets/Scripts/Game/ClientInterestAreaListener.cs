using System.Collections;
using System.Collections.Generic;
using InterestManagement;
using UnityEngine;

namespace Game.InterestManagement.Simulation
{
    [RequireComponent(typeof(ClientInterestArea))]
    public class ClientInterestAreaListener : MonoBehaviour
    {
        private INearbySceneObjectsEvents<IGameObject> nearbySceneObjectsEvents;

        private void Start()
        {
            StartCoroutine(WaitFrameAndStart());
        }

        private IEnumerator WaitFrameAndStart()
        {
            yield return null;

            var clientInterestArea = GetComponent<ClientInterestArea>();
            nearbySceneObjectsEvents =
                clientInterestArea.GetNearbySceneObjectsEvents();
            
            SubscribeToNearbyGameObjectsEvents();
        }

        private void OnDestroy()
        {
            UnsubscribeFromNearbyGameObjectsEvents();
        }

        private void SubscribeToNearbyGameObjectsEvents()
        {
            nearbySceneObjectsEvents.SceneObjectAdded += OnGameObjectAdded;
            nearbySceneObjectsEvents.SceneObjectRemoved += OnGameObjectRemoved;
            nearbySceneObjectsEvents.SceneObjectsAdded += OnGameObjectsAdded;
            nearbySceneObjectsEvents.SceneObjectsRemoved += OnGameObjectsRemoved;
        }

        private void UnsubscribeFromNearbyGameObjectsEvents()
        {
            nearbySceneObjectsEvents.SceneObjectAdded -= OnGameObjectAdded;
            nearbySceneObjectsEvents.SceneObjectRemoved -= OnGameObjectRemoved;
            nearbySceneObjectsEvents.SceneObjectsAdded -= OnGameObjectsAdded;
            nearbySceneObjectsEvents.SceneObjectsRemoved -= OnGameObjectsRemoved;
        }

        private void OnGameObjectAdded(IGameObject gameObject)
        {
            gameObject.SetGraphics(true);
        }

        private void OnGameObjectRemoved(IGameObject gameObject)
        {
            gameObject.SetGraphics(false);
        }

        private void OnGameObjectsAdded(IEnumerable<IGameObject> gameObjects)
        {
            foreach (var gameObject in gameObjects)
            {
                gameObject.SetGraphics(true);
            }
        }

        private void OnGameObjectsRemoved(IEnumerable<IGameObject> gameObjects)
        {
            foreach (var gameObject in gameObjects)
            {
                gameObject.SetGraphics(false);
            }
        }
    }
}