using Game.Messages;
using Scripts.Gameplay.Entity;
using Scripts.Services.Game;
using UnityEngine;

namespace Scripts.Gameplay.Map.Objects
{
    [RequireComponent(typeof(EntityIdentifier))]
    public class PortalTeleportation : MonoBehaviour
    {
        private int entityId;

        private void Start()
        {
            entityId = GetComponent<IEntity>().Id;
        }

        public void Teleport()
        {
            var gameApi = FindObjectOfType<GameApi>();
            var message = new ChangeSceneMessage()
            {
                PortalId = entityId
            };

            gameApi?.ChangeScene(message);
        }
    }
}