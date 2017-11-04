using Scripts.Gameplay.Actors;
using UnityEngine;

namespace Scripts.World
{
    public class PortalInteraction : MonoBehaviour
    {
        private const string PORTAL_TAG = "Portal";

        private void OnTriggerEnter2D(Collider2D collider)
        {
            var portalGameObject = collider.transform;
            if (portalGameObject.CompareTag(PORTAL_TAG))
            {
                portalGameObject.GetComponent<PortalController>().StartInteraction();
            }
        }

        private void OnTriggerExit2D(Collider2D collider)
        {
            var portalGameObject = collider.transform;
            if (portalGameObject.CompareTag(PORTAL_TAG))
            {
                portalGameObject.GetComponent<PortalController>().StopInteraction();
            }
        }
    }
}