using System;
using Game.Common;
using UnityEngine;

namespace Scripts.World.Dummy
{
    [Serializable]
    public class DummyEntity
    {
        public string Name;
        public int Id;
        public Vector2 Position;
        public Directions SpawnDirection;
    }
}