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

            gameApi.SceneEntered.Invoke(new EnteredSceneMessage()
            {
                GameObjectId = dummyCharacter.DummyEntity.Id,
                SpawnPositionData = new SpawnPositionData()
                {
                    X = dummyCharacter.DummyEntity.Position.x,
                    Y = dummyCharacter.DummyEntity.Position.y
                },
                CharacterData = new CharacterData()
                {
                    Name = "Dummy",
                    Class = (byte)dummyCharacter.CharacterClass,
                },
            });
        }
    }
}