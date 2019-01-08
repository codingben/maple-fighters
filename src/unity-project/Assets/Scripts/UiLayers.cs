using UnityEngine;

namespace UserInterface
{
    public struct UiLayers
    {
        private readonly Transform background;
        private readonly Transform foreground;

        public UiLayers(Transform background, Transform foreground)
        {
            this.background = background;
            this.foreground = foreground;
        }

        public Transform GetLayer(UiLayer uiLayer)
        {
            return uiLayer == UiLayer.Background ? background : foreground;
        }
    }
}