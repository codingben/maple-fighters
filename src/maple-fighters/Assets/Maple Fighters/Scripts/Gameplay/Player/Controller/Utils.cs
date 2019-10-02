using System.Linq;
using Scripts.Constants;
using UnityEngine;

namespace Scripts.Gameplay.Player
{
    public class Utils
    {
        public static float GetAxis(Axes axis, bool isRaw = false)
        {
            var result = default(float);

            const string VerticalName = "Vertical";
            const string HorizontalName = "Horizontal";

            switch (axis)
            {
                case Axes.Vertical:
                {
                    result =
                        isRaw
                            ? Input.GetAxisRaw(VerticalName)
                            : Input.GetAxis(VerticalName);

                    break;
                }

                case Axes.Horizontal:
                {
                    result = 
                        isRaw
                            ? Input.GetAxisRaw(HorizontalName)
                            : Input.GetAxis(HorizontalName);

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