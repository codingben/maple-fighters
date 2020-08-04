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
            var entity = GetComponent<IEntity>();
            entityId = entity.Id;
        }

        public void Teleport()
        {
            IGameApi gameApi = FindObjectOfType<GameApi>();

            var message = new ChangeSceneMessage()
            {
                PortalId = entityId
            };

            gameApi?.SendMessage(MessageCodes.ChangeScene, message);
        }
    }
}