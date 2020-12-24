using Game.Messages;
using Scripts.Services.Game;
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
            IGameApi gameApi = FindObjectOfType<GameApi>();

            // TODO: Set values
            var message = new EnterSceneMessage()
            {
                Map = 0,
                CharacterName = "Arrow",
                CharacterType = 0
            };

            gameApi?.SendMessage(MessageCodes.EnterScene, message);
        }
    }
}