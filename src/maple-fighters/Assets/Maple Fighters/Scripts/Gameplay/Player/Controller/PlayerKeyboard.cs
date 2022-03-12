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
        public KeyCode SecondaryTeleportKey = KeyCode.W;
        public KeyCode ClimbUpKey = KeyCode.UpArrow;
        public KeyCode SecondaryClimbUpKey = KeyCode.W;
        public KeyCode ClimbDownKey = KeyCode.DownArrow;
        public KeyCode SecondaryClimbDownKey = KeyCode.S;
        public KeyCode RushKey = KeyCode.Space;
        public KeyCode PrimaryAttackKey = KeyCode.LeftControl;
        public KeyCode SecondaryAttackKey = KeyCode.Z;
    }
}