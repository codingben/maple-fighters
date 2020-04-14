using UnityEngine;

namespace Scripts.Constants
{
    public static class Keyboard
    {
        public static class Axes
        {
            public const string Vertical = "Vertical";
            public const string Horizontal = "Horizontal";
        }

        public static class Keys
        {
            public const KeyCode TeleportKey = KeyCode.UpArrow;
            public const KeyCode ClimbKey = KeyCode.C;
        }
    }
}