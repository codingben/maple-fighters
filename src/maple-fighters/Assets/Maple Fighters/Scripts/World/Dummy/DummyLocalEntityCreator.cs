using System.Collections;
using Game.Common;
using Scripts.Network.Services;
using UnityEngine;

namespace Scripts.World.Dummy
{
    public class DummyLocalEntityCreator : MonoBehaviour
    {
        [SerializeField]
        private DummyEntity dummyEntity;

        [SerializeField]
        private CharacterClasses characterClass;

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
                dummyEntity.Id,
                dummyEntity.Name,
                dummyEntity.Position.x,
                dummyEntity.Position.y,
                dummyEntity.Direction);

            var character = new CharacterParameters(
                dummyEntity.Name,
                characterClass,
                CharacterIndex.Zero);

            var characterSpawnDetails = new CharacterSpawnDetailsParameters(
                dummyEntity.Id,
                character,
                dummyEntity.Direction);

            var parameters =
                new EnterSceneResponseParameters(
                    sceneObject,
                    characterSpawnDetails);

            var gameSceneApi = ServiceProvider.GameService.GetGameSceneApi();
            gameSceneApi?.SceneEntered.Invoke(parameters);
        }
    }
}