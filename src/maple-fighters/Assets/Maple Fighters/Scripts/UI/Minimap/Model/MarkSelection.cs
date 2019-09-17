using System;
using UnityEngine;

namespace Scripts.UI.Controllers
{
    [Serializable]
    public class MarkSelection
    {
        public string Name;
        public LayerMask MarkLayerMask;
    }
}