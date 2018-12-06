using UnityEngine;

namespace Assets.Scripts.Game.Utils
{
    public static class GraphicsUtils
    {
        public static void DrawRectangle(
            Vector2 position,
            Vector2 size,
            Color color)
        {
            // Right line
            Debug.DrawLine(
                position + new Vector2(size.x / 2, size.y / 2),
                position + new Vector2(size.x / 2, -(size.y / 2)),
                color);

            // Left line
            Debug.DrawLine(
                position + new Vector2(-(size.x / 2), size.y / 2),
                position + new Vector2(-(size.x / 2), -(size.y / 2)),
                color);

            // Upper line
            Debug.DrawLine(
                position + new Vector2(size.x / 2, size.y / 2),
                position + new Vector2(-(size.x / 2), size.y / 2),
                color);

            // Bottom line
            Debug.DrawLine(
                position + new Vector2(size.x / 2, -(size.y / 2)),
                position + new Vector2(-(size.x / 2), -(size.y / 2)),
                color);
        }
    }
}