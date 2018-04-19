using System;
using UnityEngine;

namespace Assets.Scripts
{
    [Serializable]
    public class DummySceneObject
    {
        public string Name;
        public int Id;
        public Vector2 Position;
        public Action<GameObject> AddComponents;
    }
}