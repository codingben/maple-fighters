using Game.Messages;
using Scripts.Gameplay.Entity;
using Scripts.Services;
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
            var gameApi = ApiProvider.ProvideGameApi();
            var message = new ChangeSceneMessage()
            {
                PortalId = entityId
            };

            gameApi?.SendMessage(MessageCodes.ChangeScene, message);
        }
    }
}