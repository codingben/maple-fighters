using System;
using Game.Common;
using UnityEngine;

namespace Scripts.World.Dummy
{
    [Serializable]
    public class DummySceneObject
    {
        public string Name;
        public int Id;
        public Vector2 Position;
        public Directions SpawnDirection;
        public Action<GameObject> AddComponentsAction;
    }
}