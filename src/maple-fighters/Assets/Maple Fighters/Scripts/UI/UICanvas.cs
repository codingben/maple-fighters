using UnityEngine;

namespace UI
{
    /// <summary>
    /// A container containing UI canvas.
    /// </summary>
    public class UICanvas
    {
        private readonly Transform background;
        private readonly Transform foreground;

        /// <summary>
        /// The container of the UI canvas.
        /// </summary>
        /// <param name="background">The background canvas.</param>
        /// <param name="foreground">The foreground canvas.</param>
        public UICanvas(Transform background, Transform foreground)
        {
            this.background = background;
            this.foreground = foreground;
        }

        /// <summary>
        /// Gets the UI canvas.
        /// </summary>
        /// <param name="uiCanvasLayer">The canvas layer.</param>
        /// <returns>The canvas.</returns>
        public Transform GetCanvas(UICanvasLayer uiCanvasLayer)
        {
            return uiCanvasLayer == UICanvasLayer.Background ? background : foreground;
        }
    }
}