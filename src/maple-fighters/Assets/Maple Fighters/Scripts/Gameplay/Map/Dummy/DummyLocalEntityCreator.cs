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
                if (networkConfiguration.IsDevelopment())
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
            var userName = UserData.CharacterData.Name;
            if (userName == null)
            {
                userName = dummyCharacter.CharacterName;
            }

            var userCharacterType = UserData.CharacterData.Type;
            if (userCharacterType == -1)
            {
                userCharacterType = (byte)dummyCharacter.CharacterClass;
            }

            var message = new EnteredSceneMessage()
            {
                GameObjectId = dummyCharacter.DummyEntity.Id,
                X = dummyCharacter.DummyEntity.Position.x,
                Y = dummyCharacter.DummyEntity.Position.y,
                CharacterName = userName,
                CharacterClass = (byte)userCharacterType
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