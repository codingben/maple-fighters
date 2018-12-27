using System;
using UnityEngine;

namespace Scripts.Gameplay.Actors
{
    [Serializable]
    public class PlayerControllerConfig
    {
        public float Speed;
        public float JumpForce;
        public float ClimbingSpeed;

        [Header("Keyboard")]
        public KeyCode JumpKey = KeyCode.Space;
    }
}