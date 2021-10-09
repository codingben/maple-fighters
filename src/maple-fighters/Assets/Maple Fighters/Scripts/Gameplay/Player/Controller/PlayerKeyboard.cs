using System;
using UnityEngine;

namespace Scripts.Gameplay.Player
{
    [Serializable]
    public class PlayerKeyboard
    {
        [Header("Keyboard")]
        public KeyCode JumpKey = KeyCode.Space;
        public KeyCode TeleportKey = KeyCode.UpArrow;
        public KeyCode ClimbUpKey = KeyCode.UpArrow;
        public KeyCode ClimbDownKey = KeyCode.DownArrow;
        public KeyCode RushKey = KeyCode.Space;
    }
}