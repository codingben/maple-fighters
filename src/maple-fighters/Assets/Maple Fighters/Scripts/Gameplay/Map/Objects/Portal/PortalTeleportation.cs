using Game.Messages;
using Scripts.Gameplay.Entity;
using Scripts.Services;
using Scripts.Services.GameApi;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scripts.Gameplay.Map.Objects
{
    [RequireComponent(typeof(EntityIdentifier))]
    public class PortalTeleportation : MonoBehaviour
    {
        private IGameApi gameApi;

        private void Awake()
        {
            gameApi = ApiProvider.ProvideGameApi();
            gameApi.SceneChanged += OnSceneChanged;
        }

        private void OnDestroy()
        {
            gameApi.SceneChanged -= OnSceneChanged;
        }

        private void OnSceneChanged(SceneChangedMessage message)
        {
            var mapIndex = (int)message.Map;

            SceneManager.LoadScene(sceneBuildIndex: mapIndex);
        }

        public void Teleport()
        {
            var entity = GetComponent<IEntity>();
            var entityId = entity.Id;
            var message = new ChangeSceneMessage()
            {
                PortalId = entityId
            };

            gameApi?.SendMessage(MessageCodes.ChangeScene, message);
        }
    }
}