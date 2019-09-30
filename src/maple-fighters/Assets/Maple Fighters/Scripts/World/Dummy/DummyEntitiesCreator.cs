using System.Collections;
using Game.Common;
using Scripts.Network.Services;
using UnityEngine;

namespace Scripts.World.Dummy
{
    public class DummyEntitiesCreator : MonoBehaviour
    {
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

                var gameSceneApi =
                    ServiceProvider.GameService.GetGameSceneApi();
                gameSceneApi?.SceneObjectAdded.Invoke(parameters);
            }
        }
    }
}