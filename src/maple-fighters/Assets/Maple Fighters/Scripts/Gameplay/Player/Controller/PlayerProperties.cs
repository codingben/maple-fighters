﻿using System;
using UnityEngine;

namespace Scripts.Gameplay.Player
{
    [Serializable]
    public class PlayerProperties
    {
        [Header("Moving")]
        public float Speed;
        public float RushSpeed;

        [Header("Jumping")]
        public float JumpHeight;
        public float JumpForce;

        [Header("Climbing")]
        public float ClimbSpeed;
    }
}