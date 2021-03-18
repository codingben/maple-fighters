using UnityEngine;
using System;

namespace UI
{
    [Serializable]
    public class UICanvasConfig
    {
        public string Name = "Canvas";
        public UICanvasType CanvasType;
        public int SortingOrder;
        public RenderMode RenderMode;
        public UICanvasScalerConfig UICanvasScalerConfig;
    }
}