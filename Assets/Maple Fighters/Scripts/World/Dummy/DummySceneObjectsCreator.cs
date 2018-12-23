using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CommonTools.Log;
using Game.Common;
using InterestManagement.Scripts;
using Scripts.Containers;
using Scripts.Gameplay;
using Scripts.Gameplay.Actors;
using Scripts.Services;
using Scripts.Utils;
using UnityEngine;

namespace Assets.Scripts
{
    public class DummySceneObjectsCreator : MonoBehaviour
    {
        [Header("Interest Area Visual Graphics")]
        private bool showVisualGraphics;

        private ISceneEvents sceneEvents;
        private IDummySceneObjectsProvider dummySceneObjectsProvider;

        private void Awake()
        {
            dummySceneObjectsProvider = GetComponent<IDummySceneObjectsProvider>().AssertNotNull();

            var sceneGameObject = GameObject.FindGameObjectWithTag(Scene.SCENE_TAG);
            sceneEvents = sceneGameObject.GetComponent<ISceneEvents>();

            if (sceneEvents != null)
            {
                sceneEvents.RegionsCreated += OnRegionsCreated;
            }
        }

        private void OnDestroy()
        {
            if (sceneEvents != null)
            {
                sceneEvents.RegionsCreated -= OnRegionsCreated;
            }

            SceneObjectsContainer.GetInstance().RemoveAllSceneObjects();
            SavedGameObjects.GetInstance().DestroyAll();
        }

        private void OnRegionsCreated()
        {
            CreateDummyPlayer();
            CreateDummySceneObjects();
        }

        private void CreateDummyPlayer()
        {
            int id;
            CreateDummyPlayerSceneObject(out id);
            CreateCommonComponentsToSceneObject(id, typeof(InterestArea), typeof(PlayerViewController));
        }

        private void CreateDummyPlayerSceneObject(out int id)
        {
            var dummyCharacterDetailsProvider = GetComponent<DummyCharacterDetailsProvider>().AssertNotNull("Could not find dummy character details.");
            var parameters = dummyCharacterDetailsProvider.GetDummyCharacterParameters();

            var gameScenePeerLogic = ServiceContainer.GameService.GetPeerLogic<IGameScenePeerLogicAPI>().AssertNotNull();
            gameScenePeerLogic.SceneEntered?.Invoke(parameters);

            id = parameters.SceneObject.Id;
        }

        private void CreateDummySceneObjects()
        {
            foreach (var dummyParameters in GetDummySceneObjectsParameters())
            {
                var gameScenePeerLogic = ServiceContainer.GameService.GetPeerLogic<IGameScenePeerLogicAPI>().AssertNotNull();
                gameScenePeerLogic.SceneObjectAdded?.Invoke(dummyParameters);
            }

            InitializeDummySceneObjects();
        }

        private void InitializeDummySceneObjects()
        {
            foreach (var dummySceneObject in dummySceneObjectsProvider.GetSceneObjects())
            {
                var id = dummySceneObject.Id;
                CreateCommonComponentsToSceneObject(id);

                var sceneObject = SceneObjectsContainer.GetInstance().GetRemoteSceneObject(id);
                if (sceneObject != null)
                {
                    dummySceneObject.AddComponents?.Invoke(sceneObject.GameObject);
                }
            }

            StartCoroutine(DisableNonPlayerEntity());
        }

        private IEnumerator DisableNonPlayerEntity()
        {
            yield return null;

            var localEntity = SceneObjectsContainer.GetInstance().GetLocalSceneObject().GameObject.GetComponent<IInterestArea>().AssertNotNull();

            foreach (var dummySceneObject in dummySceneObjectsProvider.GetSceneObjects())
            {
                var id = dummySceneObject.Id;
                var sceneObject = SceneObjectsContainer.GetInstance().GetRemoteSceneObject(id)?.GameObject;
                if (sceneObject == null)
                {
                    LogUtils.Log(MessageBuilder.Trace($"Could not find a scene object with id ${id}"));
                    continue;
                }

                var entity = sceneObject.GetComponent<ISceneObject>().AssertNotNull();
                foreach (var region in localEntity.GetSubscribedPublishers())
                {
                    if (!region.HasSubscription(entity))
                    {
                        sceneObject.gameObject.SetActive(false);
                    }
                }
            }
        }

        private void CreateCommonComponentsToSceneObject(int id, params Type[] components)
        {
            var sceneObject = SceneObjectsContainer.GetInstance().GetRemoteSceneObject(id)?.GameObject;
            if (sceneObject == null)
            {
                LogUtils.Log(MessageBuilder.Trace($"Could not find a scene object with id {id}"));
                return;
            }

            sceneObject.gameObject.name = $"{sceneObject.gameObject.name} (Id: {id})";

            if (showVisualGraphics)
            {
                sceneObject.AddComponent(typeof(InterestAreaVisualGraphics));
            }

            foreach (var component in components)
            {
                sceneObject.AddComponent(component);
            }
        }

        private IEnumerable<SceneObjectAddedEventParameters> GetDummySceneObjectsParameters()
        {
            return dummySceneObjectsProvider.GetSceneObjects()
                .Select(dummySceneObject => new SceneObjectParameters(dummySceneObject.Id, dummySceneObject.Name, dummySceneObject.Position.x, dummySceneObject.Position.y, dummySceneObject.SpawnDirection))
                .Select(parameters => new SceneObjectAddedEventParameters(parameters));
        }
    }
}