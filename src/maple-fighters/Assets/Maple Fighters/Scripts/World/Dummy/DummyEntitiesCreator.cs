using System;
using System.Collections;
using System.Collections.Generic;
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
            CreateDummyEntities();
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

        private void CreateDummyEntities()
        {
            foreach (var dummyParameters in GetDummyEntitiesParameters())
            {
                var gameSceneApi =
                    ServiceProvider.GameService.GetGameSceneApi();
                gameSceneApi?.SceneObjectAdded.Invoke(dummyParameters);
            }

            InitializeDummyEntities();
        }

        private void InitializeDummyEntities()
        {
            foreach (var dummyEntity in dummySceneObjectsProvider
                .GetSceneObjects())
            {
                var id = dummyEntity.Id;
                CreateCommonComponentsToEntity(id);

                var entity = EntityContainer.GetInstance().GetRemoteEntity(id);
                if (entity != null)
                {
                    dummyEntity.AddComponentsAction?.Invoke(entity.GameObject);
                }
            }
        }

        private void CreateCommonComponentsToEntity(
            int id,
            params Type[] components)
        {
            var entity = 
                EntityContainer.GetInstance().GetRemoteEntity(id)
                    ?.GameObject;
            if (entity == null)
            {
                Debug.LogWarning($"Could not find a scene object with id {id}");
                return;
            }

            entity.gameObject.name = $"{entity.gameObject.name} (Id: {id})";

            foreach (var component in components)
            {
                entity.AddComponent(component);
            }
        }

        private IEnumerable<SceneObjectAddedEventParameters> GetDummyEntitiesParameters()
        {
            return dummySceneObjectsProvider.GetSceneObjects()
                .Select(
                    dummySceneObject => new SceneObjectParameters(
                        dummySceneObject.Id,
                        dummySceneObject.Name,
                        dummySceneObject.Position.x,
                        dummySceneObject.Position.y,
                        dummySceneObject.SpawnDirection))
                .Select(
                    parameters =>
                        new SceneObjectAddedEventParameters(parameters));
        }
    }
}