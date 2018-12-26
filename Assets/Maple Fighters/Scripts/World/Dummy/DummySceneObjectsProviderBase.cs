using System;
using System.Collections.Generic;
using InterestManagement.Scripts;
using UnityEngine;

namespace Assets.Scripts
{
    [RequireComponent(typeof(DummySceneObjectsCreator))]
    public abstract class DummySceneObjectsProviderBase : MonoBehaviour, IDummySceneObjectsProvider
    {
        public IEnumerable<DummySceneObject> GetSceneObjects() => sceneObjects;

        private List<DummySceneObject> sceneObjects;

        private void Awake()
        {
            sceneObjects = new List<DummySceneObject>();
            sceneObjects.AddRange(GetDummySceneObjects());
        }

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

        protected abstract IEnumerable<DummySceneObject> GetDummySceneObjects();
    }
}