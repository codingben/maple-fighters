using Scripts.Constants;
using Scripts.World.Objects;
using UnityEngine;

namespace Scripts.Gameplay.Player
{
    public class PortalInteractor : MonoBehaviour
    {
        [SerializeField]
        private KeyCode interactionKey = KeyCode.LeftControl;

        private PortalTeleportation portalTeleportation;

        private void Update()
        {
            if (Input.GetKeyDown(interactionKey))
            {
                if (portalTeleportation != null)
                {
                    portalTeleportation.Teleport();
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            var transform = collider.transform;
            if (transform.CompareTag(GameTags.PortalTag))
            {
                portalTeleportation =
                    transform.GetComponent<PortalTeleportation>();
            }
        }

        private void OnTriggerExit2D(Collider2D collider)
        {
            var transform = collider.transform;
            if (transform.CompareTag(GameTags.PortalTag))
            {
                portalTeleportation = null;
            }
        }
    }
}