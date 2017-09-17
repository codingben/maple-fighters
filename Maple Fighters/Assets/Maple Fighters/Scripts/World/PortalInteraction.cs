using Scripts.Gameplay.Actors;
using UnityEngine;

namespace Scripts.World
{
    [RequireComponent(typeof(PlayerController))]
    public class PortalInteraction : MonoBehaviour
    {
        private const string PORTAL_TAG = "Portal";

        private void OnTriggerEnter2D(Collider2D collider)
        {
            var portalGameObject = collider.transform;

            if (!collider.transform.CompareTag(PORTAL_TAG))
            {
                return;
            }

            portalGameObject.GetComponent<Portal>().StartInteraction?.Invoke(gameObject.transform);
        }

        private void OnTriggerExit2D(Collider2D collider)
        {
            var portalGameObject = collider.transform;

            if (!collider.transform.CompareTag(PORTAL_TAG))
            {
                return;
            }

            portalGameObject.GetComponent<Portal>().StopInteraction?.Invoke();
        }
    }
}