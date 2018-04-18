using System;
using System.Collections.Generic;
using InterestManagement.Scripts;
using UnityEngine;

namespace Assets.Scripts
{
    [RequireComponent(typeof(DummySceneObjectsCreator))]
    public abstract class DummySceneObjectsProviderBase : MonoBehaviour, IDummySceneObjectsProvider
    {
        private readonly List<DummySceneObject> sceneObjects = new List<DummySceneObject>();

        private void Awake()
        {
            sceneObjects.AddRange(GetDummySceneObjects());
        }

        public IEnumerable<DummySceneObject> GetSceneObjects() => sceneObjects;

        protected abstract IEnumerable<DummySceneObject> GetDummySceneObjects();

        protected void AddCommonComponents(GameObject gameObject)
        {
            foreach (var component in GetCommonComponents())
            {
                gameObject.AddComponent(component);
            }
        }

        private IEnumerable<Type> GetCommonComponents()
        {
            yield return typeof(InterestArea);
        }
    }
}