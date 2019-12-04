using System;
using Game.Common;
using UnityEngine;

namespace Scripts.Gameplay.Map.Dummy
{
    [Serializable]
    public class DummyEntity
    {
        public int Id;
        public string Name;
        public Vector2 Position;
        public Directions Direction;
    }
}