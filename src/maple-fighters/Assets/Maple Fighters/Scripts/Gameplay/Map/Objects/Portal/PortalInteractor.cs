﻿using ScriptableObjects.Configurations;
using Scripts.Constants;
using UnityEngine;

namespace Scripts.Gameplay.Map.Objects
{
    public class PortalInteractor : MonoBehaviour
    {
        private PortalTeleportation portalTeleportation;
        private KeyCode teleportKey;
        private KeyCode secondaryTeleportKey;

        private void Awake()
        {
            var playerKeyboard =
                PlayerConfiguration.GetInstance().PlayerKeyboard;
            teleportKey = playerKeyboard.TeleportKey;
            secondaryTeleportKey = playerKeyboard.SecondaryTeleportKey;
        }

        private void Update()
        {
            if (Input.GetKeyDown(teleportKey) ||
                Input.GetKeyDown(secondaryTeleportKey))
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