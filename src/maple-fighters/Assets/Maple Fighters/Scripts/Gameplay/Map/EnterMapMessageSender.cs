using Game.Messages;
using Scripts.Constants;
using Scripts.Services;
using Scripts.Services.GameApi;
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
        }

        private void OnConnected()
        {
            gameApi.Connected -= OnConnected;

            EnterScene();

            Destroy(gameObject);
        }

        private void EnterScene()
        {
            var map = GetCurrentMap();
            if (map == -1)
            {
                Debug.LogWarning($"Could not get current map. Map index not found.");
                return;
            }

            var message = new EnterSceneMessage()
            {
                Map = (byte)map,
                CharacterName = UserData.CharacterData.Name,
                CharacterType = (byte)UserData.CharacterData.Type
            };

            gameApi?.SendMessage(MessageCodes.EnterScene, message);
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