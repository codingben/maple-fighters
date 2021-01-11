using System.Collections;
using Game.Messages;
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
            StartCoroutine(WaitFrameAndStart());
        }

        private IEnumerator WaitFrameAndStart()
        {
            yield return null;

            CreateLocalDummyEntity();
        }

        private void CreateLocalDummyEntity()
        {
            var gameApi = ApiProvider.ProvideGameApi();
            var message = new EnteredSceneMessage()
            {
                GameObjectId = dummyCharacter.DummyEntity.Id,
                SpawnData = new SpawnData()
                {
                    X = dummyCharacter.DummyEntity.Position.x,
                    Y = dummyCharacter.DummyEntity.Position.y
                },
                CharacterData = new CharacterData()
                {
                    Name = UserData.CharacterData.Name ?? "Dummy",
                    Class = (byte)dummyCharacter.CharacterClass,
                }
            };

            var direction = dummyCharacter.DummyEntity.Direction;
            if (direction == Player.Direction.Left)
            {
                message.SpawnData.Direction = 1;
            }
            else
            {
                message.SpawnData.Direction = -1;
            }

            gameApi.SceneEntered.Invoke(message);
        }
    }
}