using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game.Common;
using Scripts.Containers;
using Scripts.Gameplay.Actors;
using Scripts.Network.Services;
using Scripts.Utils;
using UnityEngine;

namespace Scripts.World.Dummy
{
    [RequireComponent(typeof(DummyCharacterDetailsProvider))]
    public class DummySceneObjectsCreator : MonoBehaviour
    {
        private IDummySceneObjectsProvider dummySceneObjectsProvider;

        private void Awake()
        {
            dummySceneObjectsProvider =
                GetComponent<IDummySceneObjectsProvider>();
        }

        private void Start()
        {
            StartCoroutine(WaitFrameAndStart());
        }

        private void OnDestroy()
        {
            SavedGameObjects.GetInstance().DestroyAll();
        }

        private IEnumerator WaitFrameAndStart()
        {
            yield return null;

            CreateDummyPlayerSceneObject(out _);
            CreateDummySceneObjects();
        }

        private void CreateDummyPlayerSceneObject(out int id)
        {
            var dummyCharacterDetailsProvider =
                GetComponent<DummyCharacterDetailsProvider>();
            var parameters = 
                dummyCharacterDetailsProvider.GetDummyCharacterParameters();

            var gameSceneApi = ServiceProvider.GameService.GetGameSceneApi();
            gameSceneApi?.SceneEntered.Invoke(parameters);

            id = parameters.SceneObject.Id;
        }

        private void CreateDummySceneObjects()
        {
            foreach (var dummyParameters in GetDummySceneObjectsParameters())
            {
                var gameSceneApi =
                    ServiceProvider.GameService.GetGameSceneApi();
                gameSceneApi?.SceneObjectAdded.Invoke(dummyParameters);
            }

            InitializeDummySceneObjects();
        }

        private void InitializeDummySceneObjects()
        {
            foreach (var dummySceneObject in dummySceneObjectsProvider
                .GetSceneObjects())
            {
                var id = dummySceneObject.Id;
                CreateCommonComponentsToSceneObject(id);

                var entity = EntityContainer.GetInstance().GetRemoteEntity(id);
                if (entity != null)
                {
                    dummySceneObject.AddComponentsAction?.Invoke(
                        entity.GameObject);
                }
            }
        }

        private void CreateCommonComponentsToSceneObject(
            int id,
            params Type[] components)
        {
            var sceneObject = 
                EntityContainer.GetInstance().GetRemoteEntity(id)
                    ?.GameObject;
            if (sceneObject == null)
            {
                Debug.LogWarning($"Could not find a scene object with id {id}");
                return;
            }

            sceneObject.gameObject.name =
                $"{sceneObject.gameObject.name} (Id: {id})";

            foreach (var component in components)
            {
                sceneObject.AddComponent(component);
            }
        }

        private IEnumerable<SceneObjectAddedEventParameters> GetDummySceneObjectsParameters()
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