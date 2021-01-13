using System.Collections;
using Game.Messages;
using Scripts.Services;
using UnityEngine;

namespace Scripts.Gameplay.Map
{
    public class EnterMapMessageSender : MonoBehaviour
    {
        private void Start()
        {
            StartCoroutine(WaitSecondAndSendEnterScene());
        }

        // TODO: Refactor
        private IEnumerator WaitSecondAndSendEnterScene()
        {
            yield return new WaitForSeconds(0.25f);

            EnterScene();

            Destroy(gameObject);
        }

        private void EnterScene()
        {
            var gameApi = ApiProvider.ProvideGameApi();
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