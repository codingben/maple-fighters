using System.Collections;
using Game.Common;
using Scripts.Services.Game;
using UnityEngine;

namespace Scripts.World.Dummy
{
    public class DummyEntitiesCreator : MonoBehaviour
    {
        [SerializeField]
        private DummyEntity[] dummyEntities;

        private void Start()
        {
            StartCoroutine(WaitFrameAndStart());
        }

        private IEnumerator WaitFrameAndStart()
        {
            yield return null;

            CreateDummyEntities();
        }

        private void CreateDummyEntities()
        {
            foreach (var dummyEntity in dummyEntities)
            {
                var parameters = new SceneObjectAddedEventParameters(
                    new SceneObjectParameters(
                        dummyEntity.Id,
                        dummyEntity.Name,
                        dummyEntity.Position.x,
                        dummyEntity.Position.y,
                        dummyEntity.Direction));

                var gameService = GameService.GetInstance();
                gameService?.GameSceneApi.SceneObjectAdded.Invoke(parameters);
            }
        }
    }
}