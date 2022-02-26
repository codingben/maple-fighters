using Game.Messages;
using Scripts.Constants;
using Scripts.Services;
using Scripts.Services.GameApi;
using Scripts.UI.ScreenFade;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scripts.Gameplay.Map
{
    public class EnterMapMessageSender : MonoBehaviour
    {
        private IGameApi gameApi;

        private void Awake()
        {
            gameApi = ApiProvider.ProvideGameApi();
            gameApi.Connected += OnConnected;
            gameApi.SceneEntered.AddListener(OnSceneEntered);
        }

        private void OnConnected()
        {
            gameApi.Connected -= OnConnected;

            EnterScene();
        }

        private void EnterScene()
        {
            var map = GetCurrentMap();
            if (map == -1)
            {
                Debug.Log("Could not get current map. Map index not found.");
                return;
            }

            var userMetadata = FindObjectOfType<UserMetadata>();
            if (userMetadata == null)
            {
                Debug.Log("Could not find user metadata to enter scene.");
                return;
            }

            var message = new EnterSceneMessage()
            {
                Map = (byte)map,
                CharacterName = userMetadata.CharacterName,
                CharacterType = (byte)userMetadata.CharacterType
            };

            gameApi?.SendMessage(MessageCodes.EnterScene, message);
        }

        private void OnSceneEntered(EnteredSceneMessage _)
        {
            var screenFadeController =
                FindObjectOfType<ScreenFadeController>();
            screenFadeController?.Hide();
        }

        private int GetCurrentMap()
        {
            var scene = SceneManager.GetActiveScene();
            var index = scene.buildIndex;

            switch (index)
            {
                case SceneIndexes.Lobby: return 0; // Map 1
                case SceneIndexes.TheDarkForest: return 1; // Map 2
                default: return -1;
            }
        }
    }
}