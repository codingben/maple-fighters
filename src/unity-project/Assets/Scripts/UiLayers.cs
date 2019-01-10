using UnityEngine;

namespace UI.Manager
{
    public struct UILayers
    {
        private readonly Transform background;
        private readonly Transform foreground;

        public UILayers(Transform background, Transform foreground)
        {
            this.background = background;
            this.foreground = foreground;
        }

        public Transform GetLayer(UILayer uiLayer)
        {
            return uiLayer == UILayer.Background ? background : foreground;
        }
    }
}