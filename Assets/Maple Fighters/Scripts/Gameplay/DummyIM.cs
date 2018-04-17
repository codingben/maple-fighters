using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CommonTools.Log;
using Game.Common;
using InterestManagement.Scripts;
using Scripts.Containers;
using Scripts.Gameplay.Actors;
using Scripts.Services;
using UnityEngine;

namespace Assets.Scripts
{
    [Serializable]
    public class DummySceneObject
    {
        public string Name;
        public Vector2 Position;
    }

    public class DummyIM : MonoBehaviour
    {
        [SerializeField] private DummySceneObject[] dummySceneObjects;

        [Header("Visual Graphics")]
        public bool showVisualGraphics;

        private int id;
        private ISceneEvents sceneEvents;

        private void Awake()
        {
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
            var dummySceneObject = SceneObjectsContainer.Instance.GetRemoteSceneObject(id)
                .AssertNotNull(MessageBuilder.Trace($"Could not find a scene object with id ${id}"))?.GetGameObject();
            if (dummySceneObject == null)
            {
                return;
            }

            var components = new List<Type>
            {
                typeof(Entity),
                typeof(InterestArea),
                typeof(NearbySubscribers),
                typeof(PlayerViewController)
            };

            if (showVisualGraphics)
            {
                components.Add(typeof(InterestAreaVisualGraphics));
            }

            AddComponents(dummySceneObject, components);

            var interestAreaVisualGraphics = dummySceneObject.GetComponent<InterestAreaVisualGraphics>();
            if (interestAreaVisualGraphics != null)
            {
                const int Z = -1;
                interestAreaVisualGraphics.InterestAreaGraphics.transform.localScale = new Vector3(
                    interestAreaVisualGraphics.InterestAreaGraphics.transform.localScale.x,
                    interestAreaVisualGraphics.InterestAreaGraphics.transform.localScale.y,
                    Z
                );
            }
        }

        private void CreateDummySceneObjects()
        {
            foreach (var dummyParameters in GetDummySceneObjects())
            {
                var gameScenePeerLogic = ServiceContainer.GameService.GetPeerLogic<IGameScenePeerLogicAPI>().AssertNotNull();
                gameScenePeerLogic.SceneObjectAdded?.Invoke(dummyParameters);
            }

            for (var i = 1; i < (id + 1); ++i)
            {
                var dummySceneObject = SceneObjectsContainer.Instance.GetRemoteSceneObject(i)
                    .AssertNotNull(MessageBuilder.Trace($"Could not find a scene object with id ${i}"))?.GetGameObject();
                if (dummySceneObject == null)
                {
                    continue;
                }

                var components = new List<Type>
                {
                    typeof(Entity),
                    typeof(InterestArea),
                    typeof(NearbySubscribers)
                };

                if (showVisualGraphics)
                {
                    components.Add(typeof(InterestAreaVisualGraphics));
                }

                AddComponents(dummySceneObject, components);

                var interestAreaVisualGraphics = dummySceneObject.GetComponent<InterestAreaVisualGraphics>();
                if (interestAreaVisualGraphics != null)
                {
                    const int Z = -1;
                    interestAreaVisualGraphics.InterestAreaGraphics.transform.localScale = new Vector3(
                        interestAreaVisualGraphics.InterestAreaGraphics.transform.localScale.x,
                        interestAreaVisualGraphics.InterestAreaGraphics.transform.localScale.y,
                        Z
                    );
                }
            }

            StartCoroutine(DisableNonPlayerEntity());
        }

        private IEnumerator DisableNonPlayerEntity()
        {
            yield return null;

            var localEntity = SceneObjectsContainer.Instance.GetLocalSceneObject().GetGameObject().GetComponent<IInterestArea>().AssertNotNull();

            for (var i = 1; i < (id + 1); ++i)
            {
                var dummySceneObject = SceneObjectsContainer.Instance.GetRemoteSceneObject(i)
                    .AssertNotNull(MessageBuilder.Trace($"Could not find a scene object with id ${i}"))?.GetGameObject();
                var entity = dummySceneObject?.GetComponent<ISceneObject>().AssertNotNull();

                foreach (var region in localEntity.GetSubscribedPublishers())
                {
                    if (!region.HasSubscription(entity))
                    {
                        dummySceneObject?.gameObject.SetActive(false);
                    }
                }
            }
        }

        private void AddComponents(GameObject dummySceneObject, IEnumerable<Type> components)
        {
            foreach (var component in components)
            {
                dummySceneObject.AddComponent(component);
            }
        }

        private IEnumerable<SceneObjectAddedEventParameters> GetDummySceneObjects()
        {
            return dummySceneObjects.Select(dummySceneObject => new SceneObjectParameters(++id, dummySceneObject.Name, dummySceneObject.Position.x, dummySceneObject.Position.y))
                .Select(parameters => new SceneObjectAddedEventParameters(parameters));
        }
    }
}