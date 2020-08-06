using System.Linq;
using Scripts.Constants;
using UnityEngine;

namespace Scripts.Gameplay.Player
{
    public static class Utils
    {
        public static float GetAxis(Axes axis, bool isRaw = false)
        {
            var result = default(float);

            switch (axis)
            {
                case Axes.Vertical:
                    {
                        result =
                            isRaw
                                ? Input.GetAxisRaw(Keyboard.Axes.Vertical)
                                : Input.GetAxis(Keyboard.Axes.Vertical);

                        break;
                    }

                case Axes.Horizontal:
                    {
                        result =
                            isRaw
                                ? Input.GetAxisRaw(Keyboard.Axes.Horizontal)
                                : Input.GetAxis(Keyboard.Axes.Horizontal);

                        break;
                    }
            }

            return result;
        }

        public static Collider2D GetGroundedCollider(Vector2 position)
        {
            var hit = Physics2D.RaycastAll(
                position,
                Vector2.down,
                1,
                1 << LayerMask.NameToLayer(GameLayers.FloorLayer));

            Collider2D collider = null;

            if (hit.Any())
            {
                collider = hit.First().collider;
            }

            return collider;
        }
    }
}