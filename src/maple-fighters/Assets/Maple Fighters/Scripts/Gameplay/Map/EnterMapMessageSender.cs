using Game.Messages;
using Scripts.Services;
using UnityEngine;

namespace Scripts.Gameplay.Map
{
    public class EnterMapMessageSender : MonoBehaviour
    {
        private void Start()
        {
            EnterScene();

            Destroy(gameObject);
        }

        private void EnterScene()
        {
            var gameApi = ApiProvider.ProvideGameApi();
            var message = new EnterSceneMessage()
            {
                CharacterName = UserData.CharacterData.Name,
                CharacterType = UserData.CharacterData.Type
            };

            gameApi?.SendMessage(MessageCodes.EnterScene, message);
        }
    }
}