using System.Linq;
using Game.Common;
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

        public static void SetLocalScaleByDirection(ref Transform transform, Directions direction)
        {
            var x = Mathf.Abs(transform.localScale.x);
            var y = transform.localScale.y;
            var z = transform.localScale.z;

            switch (direction)
            {
                case Directions.Left:
                {
                    transform.localScale = new Vector3(x, y, z);
                    break;
                }

                case Directions.Right:
                {
                    transform.localScale = new Vector3(-x, y, z);
                    break;
                }
            }
        }
    }
}