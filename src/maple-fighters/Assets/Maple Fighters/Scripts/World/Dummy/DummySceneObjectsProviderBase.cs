using System.Collections.Generic;
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

        protected abstract IEnumerable<DummySceneObject> GetDummySceneObjects();
    }
}