using System;
using UnityEngine;

namespace Scripts.UI.Minimap
{
    [Serializable]
    public class MarkSelection
    {
        public string Name;
        public LayerMask MarkLayerMask;
    }
}