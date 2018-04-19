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
        public bool showVisualGraphics;

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

            LoadedObjects.DestroyAll();
        }

        private void OnRegionsCreated()
        {
            CreateDummyPlayerCharacter();
            CreateDummySceneObjects();
        }

        private void CreateDummyPlayerCharacter()
        {
            var dummyCharacterDetailsProvider = GetComponent<DummyCharacterDetailsProvider>()
                .AssertNotNull("Could not find dummy character details. Please add DummyCharacterDetails into a scene.");
            var parameters = dummyCharacterDetailsProvider.GetDummyCharacterParameters();

            var gameScenePeerLogic = ServiceContainer.GameService.GetPeerLogic<IGameScenePeerLogicAPI>().AssertNotNull();
            gameScenePeerLogic.SceneEntered?.Invoke(parameters);

            var id = parameters.SceneObject.Id;
            var sceneObject = SceneObjectsContainer.Instance.GetRemoteSceneObject(id)?.GetGameObject();
            if (sceneObject == null)
            {
                LogUtils.Log(MessageBuilder.Trace($"Could not find a scene object with id ${id}"));
                return;
            }

            sceneObject.gameObject.name = $"{sceneObject.gameObject.name} (Id: {id})";

            if (showVisualGraphics)
            {
                sceneObject.AddComponent(typeof(InterestAreaVisualGraphics));
            }

            sceneObject.AddComponent(typeof(InterestArea));
            sceneObject.AddComponent(typeof(PlayerViewController));
        }

        private void CreateDummySceneObjects()
        {
            foreach (var dummyParameters in GetDummySceneObjectsParameters())
            {
                var gameScenePeerLogic = ServiceContainer.GameService.GetPeerLogic<IGameScenePeerLogicAPI>().AssertNotNull();
                gameScenePeerLogic.SceneObjectAdded?.Invoke(dummyParameters);
            }

            foreach (var dummySceneObject in dummySceneObjectsProvider.GetSceneObjects())
            {
                var id = dummySceneObject.Id;
                var sceneObject = SceneObjectsContainer.Instance.GetRemoteSceneObject(id)?.GetGameObject();
                if (sceneObject == null)
                {
                    LogUtils.Log(MessageBuilder.Trace($"Could not find a scene object with id ${id}"));
                    continue;
                }

                sceneObject.gameObject.name = $"{sceneObject.gameObject.name} (Id: {id})";

                if (showVisualGraphics)
                {
                    sceneObject.AddComponent(typeof(InterestAreaVisualGraphics));
                }

                dummySceneObject.AddComponents?.Invoke(sceneObject);
            }

            StartCoroutine(DisableNonPlayerEntity());
        }

        private IEnumerator DisableNonPlayerEntity()
        {
            yield return null;

            var localEntity = SceneObjectsContainer.Instance.GetLocalSceneObject().GetGameObject().GetComponent<IInterestArea>().AssertNotNull();

            foreach (var dummySceneObject in dummySceneObjectsProvider.GetSceneObjects())
            {
                var id = dummySceneObject.Id;
                var sceneObject = SceneObjectsContainer.Instance.GetRemoteSceneObject(id)?.GetGameObject();
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

        private IEnumerable<SceneObjectAddedEventParameters> GetDummySceneObjectsParameters()
        {
            return dummySceneObjectsProvider.GetSceneObjects()
                .Select(dummySceneObject => new SceneObjectParameters(dummySceneObject.Id, dummySceneObject.Name, dummySceneObject.Position.x, dummySceneObject.Position.y))
                .Select(parameters => new SceneObjectAddedEventParameters(parameters));
        }
    }
}