using UnityEngine;

namespace Scripts.Gameplay.Actors
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
    }
}