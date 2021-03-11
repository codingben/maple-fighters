using Game.Messages;
using Scripts.Services;
using Scripts.Services.GameApi;
using UnityEngine;

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
            var message = new EnterSceneMessage()
            {
                // TODO: Don't hard code
                Map = 0,
                CharacterName = UserData.CharacterData.Name,
                CharacterType = (byte)UserData.CharacterData.Type
            };

            gameApi?.SendMessage(MessageCodes.EnterScene, message);
        }
    }
}