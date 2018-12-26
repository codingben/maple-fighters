using UnityEngine;

namespace Scripts.World
{
    public class PortalInteraction : MonoBehaviour
    {
        private const string PortalTag = "Portal";

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.transform.CompareTag(PortalTag))
            {
                var portalController = 
                    collider.transform.GetComponent<IPortalControllerBase>();
                portalController.StartInteraction();
            }
        }

        private void OnTriggerExit2D(Collider2D collider)
        {
            if (collider.transform.CompareTag(PortalTag))
            {
                var portalController = 
                    collider.transform.GetComponent<IPortalControllerBase>();
                portalController.StopInteraction();
            }
        }
    }
}