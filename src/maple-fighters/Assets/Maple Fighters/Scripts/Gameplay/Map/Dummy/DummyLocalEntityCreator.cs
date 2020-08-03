using System.Collections;
using Game.Messages;
using Scripts.Services.Game;
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
            var gameApi = FindObjectOfType<GameApi>();
            gameApi.SceneEntered.Invoke(new EnteredSceneMessage()
            {
                GameObjectId = dummyCharacter.DummyEntity.Id,
                SpawnPositionData = new SpawnPositionData()
                {
                    X = dummyCharacter.DummyEntity.Position.x,
                    Y = dummyCharacter.DummyEntity.Position.y
                }
            });

            // TODO: Remove old code
            /*var sceneObject = new SceneObjectParameters(
                dummyCharacter.DummyEntity.Id,
                dummyCharacter.DummyEntity.Type.ToString(),
                dummyCharacter.DummyEntity.Position.x,
                dummyCharacter.DummyEntity.Position.y,
                dummyCharacter.DummyEntity.Direction);

            var character = new CharacterParameters(
                dummyCharacter.DummyEntity.Type.ToString(),
                dummyCharacter.CharacterClass,
                dummyCharacter.CharacterIndex);

            var characterSpawnDetails = new CharacterSpawnDetailsParameters(
                dummyCharacter.DummyEntity.Id,
                character,
                dummyCharacter.DummyEntity.Direction);

            var parameters =
                new EnterSceneResponseParameters(
                    sceneObject,
                    characterSpawnDetails);

            var gameService = FindObjectOfType<GameService>();
            gameService?.GameSceneApi?.SceneEntered.Invoke(parameters);*/
        }
    }
}