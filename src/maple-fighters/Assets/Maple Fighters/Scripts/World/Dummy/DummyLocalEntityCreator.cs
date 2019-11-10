using System.Collections;
using Game.Common;
using Scripts.Services.Game;
using UnityEngine;

namespace Scripts.World.Dummy
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
            var sceneObject = new SceneObjectParameters(
                dummyCharacter.DummyEntity.Id,
                dummyCharacter.DummyEntity.Name,
                dummyCharacter.DummyEntity.Position.x,
                dummyCharacter.DummyEntity.Position.y,
                dummyCharacter.DummyEntity.Direction);

            var character = new CharacterParameters(
                dummyCharacter.DummyEntity.Name,
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
            gameService?.GameSceneApi.SceneEntered.Invoke(parameters);
        }
    }
}