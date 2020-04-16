using System;
using UnityEngine;

namespace Scripts.Gameplay.Player
{
    [Serializable]
    public class PlayerKeyboard
    {
        [Header("Keyboard")]
        public KeyCode JumpKey = KeyCode.Space;
        public KeyCode TeleportKey = KeyCode.T;
        public KeyCode ClimbKey = KeyCode.C;
    }
}