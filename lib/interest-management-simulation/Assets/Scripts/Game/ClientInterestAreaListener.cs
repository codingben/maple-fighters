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
            nearbySceneObjectsEvents.SceneObjectAdded += OnSceneObjectAdded;
            nearbySceneObjectsEvents.SceneObjectRemoved += OnSceneObjectRemoved;
            nearbySceneObjectsEvents.SceneObjectsAdded += OnSceneObjectsAdded;
            nearbySceneObjectsEvents.SceneObjectsRemoved += OnSceneObjectsRemoved;
        }

        private void UnsubscribeFromNearbyGameObjectsEvents()
        {
            nearbySceneObjectsEvents.SceneObjectAdded -= OnSceneObjectAdded;
            nearbySceneObjectsEvents.SceneObjectRemoved -= OnSceneObjectRemoved;
            nearbySceneObjectsEvents.SceneObjectsAdded -= OnSceneObjectsAdded;
            nearbySceneObjectsEvents.SceneObjectsRemoved -= OnSceneObjectsRemoved;
        }

        private void OnSceneObjectAdded(IGameObject gameObject)
        {
            gameObject.SetGraphics(true);
        }

        private void OnSceneObjectRemoved(IGameObject gameObject)
        {
            gameObject.SetGraphics(false);
        }

        private void OnSceneObjectsAdded(IEnumerable<IGameObject> gameObjects)
        {
            foreach (var gameObject in gameObjects)
            {
                gameObject.SetGraphics(true);
            }
        }

        private void OnSceneObjectsRemoved(IEnumerable<IGameObject> gameObjects)
        {
            foreach (var gameObject in gameObjects)
            {
                gameObject.SetGraphics(false);
            }
        }
    }
}