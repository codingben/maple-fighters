using System.Collections;
using Game.Messages;
using ScriptableObjects.Configurations;
using Scripts.Services;
using UnityEngine;

namespace Scripts.Gameplay.Map.Dummy
{
    public class DummyLocalEntityCreator : MonoBehaviour
    {
        [SerializeField]
        private DummyCharacter dummyCharacter;

        private void Start()
        {
            var networkConfiguration = NetworkConfiguration.GetInstance();
            if (networkConfiguration != null)
            {
                if (networkConfiguration.IsEditor())
                {
                    StartCoroutine(WaitFrameAndStart());
                }
            }
        }

        private IEnumerator WaitFrameAndStart()
        {
            yield return null;

            CreateLocalDummyEntity();
        }

        private void CreateLocalDummyEntity()
        {
            var userMetadata = FindObjectOfType<UserMetadata>();
            var characterName = userMetadata?.CharacterName;
            if (characterName == null)
            {
                characterName = dummyCharacter.CharacterName;
            }

            var characterType = userMetadata?.CharacterType;
            if (characterType == null)
            {
                characterType = (byte)dummyCharacter.CharacterClass;
            }

            var message = new EnteredSceneMessage()
            {
                GameObjectId = dummyCharacter.DummyEntity.Id,
                X = dummyCharacter.DummyEntity.Position.x,
                Y = dummyCharacter.DummyEntity.Position.y,
                CharacterName = characterName,
                CharacterClass = (byte)characterType
            };

            var direction = dummyCharacter.DummyEntity.Direction;
            if (direction == Player.Direction.Left)
            {
                message.Direction = 1;
            }
            else
            {
                message.Direction = -1;
            }

            var gameApi = ApiProvider.ProvideGameApi();
            gameApi.SceneEntered.Invoke(message);
        }
    }
}