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
using UnityEngine;

namespace Assets.Scripts
{
    public class DummySceneObjectsCreator : MonoBehaviour
    {
        [Header("Visual Graphics")]
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
        }

        private void OnRegionsCreated()
        {
            CreateDummyPlayerCharacter();
            CreateDummySceneObjects();
        }

        private void CreateDummyPlayerCharacter()
        {
            var parameters = DummyCharacterDetails.Instance.AssertNotNull("Could not find dummy character details. Please add DummyCharacterDetails into a scene.")
                .GetDummyCharacterParameters();

            var gameScenePeerLogic = ServiceContainer.GameService.GetPeerLogic<IGameScenePeerLogicAPI>().AssertNotNull();
            gameScenePeerLogic.SceneEntered?.Invoke(parameters);

            var id = parameters.SceneObject.Id;
            var dummySceneObject = SceneObjectsContainer.Instance.GetRemoteSceneObject(id)?.GetGameObject();
            if (dummySceneObject == null)
            {
                LogUtils.Log(MessageBuilder.Trace($"Could not find a scene object with id ${id}"));
                return;
            }

            if (showVisualGraphics)
            {
                dummySceneObject.AddComponent(typeof(InterestAreaVisualGraphics));
            }

            dummySceneObject.AddComponent(typeof(InterestArea));
            dummySceneObject.AddComponent(typeof(PlayerViewController));
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