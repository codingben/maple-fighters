using System.Collections;
using Game.Common;
using Scripts.Gameplay.Actors;
using Scripts.Gameplay.Entity;
using Scripts.Network.Services;
using Scripts.Utils;
using UnityEngine;

namespace Scripts.World.Dummy
{
    [RequireComponent(typeof(DummyCharacterDetailsProvider))]
    public class DummyEntitiesCreator : MonoBehaviour
    {
        private IDummyEntitiesProvider dummyEntitiesProvider;

        private void Awake()
        {
            dummyEntitiesProvider = GetComponent<IDummyEntitiesProvider>();
        }

        private void Start()
        {
            StartCoroutine(WaitFrameAndStart());
        }

        private void OnDestroy()
        {
            // TODO: WTF?
            SavedGameObjects.GetInstance().DestroyAll();
        }

        private IEnumerator WaitFrameAndStart()
        {
            yield return null;

            CreateLocalDummyEntity();
            StartCoroutine(CreateDummyEntities());
        }

        private void CreateLocalDummyEntity()
        {
            var dummyCharacterDetailsProvider =
                GetComponent<DummyCharacterDetailsProvider>();
            var parameters = 
                dummyCharacterDetailsProvider.GetDummyCharacterParameters();
            var gameSceneApi = ServiceProvider.GameService.GetGameSceneApi();
            gameSceneApi?.SceneEntered.Invoke(parameters);
        }

        private IEnumerator CreateDummyEntities()
        {
            var dummyEntities = dummyEntitiesProvider.GetEntities();
            foreach (var dummyEntity in dummyEntities)
            {
                // Create the entity
                var dummyParameters = new SceneObjectAddedEventParameters(
                    new SceneObjectParameters(
                        dummyEntity.Id,
                        dummyEntity.Name,
                        dummyEntity.Position.x,
                        dummyEntity.Position.y,
                        dummyEntity.SpawnDirection));
                var gameSceneApi =
                    ServiceProvider.GameService.GetGameSceneApi();
                gameSceneApi?.SceneObjectAdded.Invoke(dummyParameters);

                // Wait a frame
                yield return null;

                // Get the entity
                var id = dummyEntity.Id;
                var entity = EntityContainer.GetInstance().GetRemoteEntity(id);
                if (entity != null)
                {
                    entity.GameObject.name =
                        $"{entity.GameObject.name} (Id: {id})";

                    // TODO: Refactor this shit
                    dummyEntity.AddComponentsAction?.Invoke(entity.GameObject);
                }
            }
        }
    }
}