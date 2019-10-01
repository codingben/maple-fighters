using Scripts.World;
using UnityEngine;

namespace Scripts.Gameplay.Player
{
    public class PortalInteractor : MonoBehaviour
    {
        private const string PortalTag = "Portal";

        private void Update()
        {
            // TODO: Implement
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.transform.CompareTag(PortalTag))
            {
                var portalController =
                    collider.transform.GetComponent<PortalTeleportation>();
                if (portalController != null)
                {
                    portalController.Teleport();
                }
            }
        }
    }
}