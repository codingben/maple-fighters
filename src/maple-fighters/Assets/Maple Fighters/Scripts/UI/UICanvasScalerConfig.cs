using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    using ScaleMode = CanvasScaler.ScaleMode;
    using ScreenMatchMode = CanvasScaler.ScreenMatchMode;

    [Serializable]
    public class UICanvasScalerConfig
    {
        public ScaleMode ScaleMode;
        public Vector2 ReferenceResolution;
        public ScreenMatchMode ScreenMatchMode;
        public float MatchWidthOrHeight;
        public float ReferencePixelsPerUnit;
    }
}