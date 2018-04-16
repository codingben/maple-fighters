using UnityEngine;

namespace InterestManagement.Scripts
{
    public class PlayerViewController : MonoBehaviour
    {
        private IInterestAreaEvents interestAreaEvents;

        private void Start()
        {
            SubscribeToInterestAreaEvents();
        }

        private void OnDestroy()
        {
            UnsubscribeFromInterestAreaEvents();
        }

        private void SubscribeToInterestAreaEvents()
        {
            interestAreaEvents = GetComponent<IInterestAreaEvents>();
            if (interestAreaEvents != null)
            {
                interestAreaEvents.SubscribersAdded += OnSubscribersAdded;
                interestAreaEvents.SubscribersRemoved += OnSubscribersRemoved;
            }
        }

        private void UnsubscribeFromInterestAreaEvents()
        {
            if (interestAreaEvents != null)
            {
                interestAreaEvents.SubscribersAdded -= OnSubscribersAdded;
                interestAreaEvents.SubscribersRemoved -= OnSubscribersRemoved;
            }
        }

        private void OnSubscribersAdded(ISceneObject[] sceneObjects)
        {
            foreach (var sceneObject in sceneObjects)
            {
                sceneObject.GetGameObject().SetActive(true);
            }
        }

        private void OnSubscribersRemoved(ISceneObject[] sceneObjects)
        {
            foreach (var sceneObject in sceneObjects)
            {
                sceneObject.GetGameObject().SetActive(false);
            }
        }
    }
}