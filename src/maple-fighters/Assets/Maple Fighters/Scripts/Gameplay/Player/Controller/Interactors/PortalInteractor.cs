using Scripts.Constants;
using Scripts.World.Objects;
using UnityEngine;

namespace Scripts.Gameplay.Player
{
    public class PortalInteractor : MonoBehaviour
    {
        [SerializeField]
        private KeyCode key = KeyCode.LeftControl;

        private PortalTeleportation portalTeleportation;

        private void Update()
        {
            if (Input.GetKeyDown(key))
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