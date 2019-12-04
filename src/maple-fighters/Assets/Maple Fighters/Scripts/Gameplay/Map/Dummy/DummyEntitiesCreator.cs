using System.Collections;
using Game.Common;
using Scripts.Services.Game;
using UnityEngine;

namespace Scripts.Gameplay.Map.Dummy
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

                var gameService = FindObjectOfType<GameService>();
                gameService?.GameSceneApi.SceneObjectAdded.Invoke(parameters);
            }
        }
    }
}