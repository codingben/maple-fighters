using System;
using UnityEngine;

namespace Scripts.Gameplay.Player
{
    [Serializable]
    public class PlayerProperties
    {
        [Header("Move")]
        public float Speed;
        public float JumpHeight;
        public float JumpForce;

        [Header("Climbing")]
        public float RopeSpeed;
        public float LadderSpeed;

        [Header("Keyboard")]
        public KeyCode JumpKey = KeyCode.Space;
    }
}