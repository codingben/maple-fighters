using Game.Messages;
using Scripts.Constants;
using Scripts.Gameplay.Entity;
using Scripts.Services;
using Scripts.Services.GameApi;
using Scripts.UI.ScreenFade;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scripts.Gameplay.Map.Objects
{
    [RequireComponent(typeof(EntityIdentifier))]
    public class PortalTeleportation : MonoBehaviour
    {
        private IGameApi gameApi;
        private int mapIndex;

        private void Start()
        {
            gameApi = ApiProvider.ProvideGameApi();
            gameApi.SceneChanged.AddListener(OnSceneChanged);
        }

        private void OnDisable()
        {
            gameApi?.SceneChanged?.RemoveListener(OnSceneChanged);
        }

        private void OnSceneChanged(SceneChangedMessage message)
        {
            mapIndex = (int)message.Map;

            switch (mapIndex)
            {
                case 0:
                {
                    SceneManager.LoadScene(sceneName: SceneNames.Maps.Lobby);
                    break;
                }

                case 1:
                {
                    SceneManager.LoadScene(sceneName: SceneNames.Maps.TheDarkForest);
                    break;
                }
            }
        }

        public void Teleport()
        {
            var screenFadeController = FindObjectOfType<ScreenFadeController>();
            if (screenFadeController != null)
            {
                screenFadeController.FadeInCompleted = () =>
                {
                    var entity = GetComponent<IEntity>();
                    var entityId = entity.Id;
                    var message = new ChangeSceneMessage()
                    {
                        PortalId = entityId
                    };

                    gameApi?.SendMessage(MessageCodes.ChangeScene, message);
                };
                screenFadeController.Show();
            }
        }
    }
}