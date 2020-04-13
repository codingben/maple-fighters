using Scripts.Constants;
using UnityEngine;

namespace Scripts.Gameplay.Map.Objects
{
    public class PortalInteractor : MonoBehaviour
    {
        private PortalTeleportation portalTeleportation;

        private void Update()
        {
            if (Input.GetKeyDown(Keyboard.Keys.TeleportKey))
            {
                portalTeleportation?.Teleport();
            }
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.transform.CompareTag(GameTags.PortalTag))
            {
                portalTeleportation =
                    collider.transform.GetComponent<PortalTeleportation>();
            }
        }

        private void OnTriggerExit2D(Collider2D collider)
        {
            if (collider.transform.CompareTag(GameTags.PortalTag))
            {
                portalTeleportation = null;
            }
        }
    }
}