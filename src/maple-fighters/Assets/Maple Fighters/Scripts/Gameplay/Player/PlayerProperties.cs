using System;
using UnityEngine;

namespace Scripts.Gameplay.Actors
{
    [Serializable]
    public class PlayerProperties
    {
        public float Speed;
        public float JumpHeight;
        public float JumpForce;
        public float RopeClimbingSpeed;
        public float LadderClimbingSpeed;

        [Header("Keyboard")]
        public KeyCode JumpKey = KeyCode.Space;
    }
}