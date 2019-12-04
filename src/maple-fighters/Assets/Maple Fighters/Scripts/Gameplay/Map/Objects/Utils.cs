using UnityEngine;

namespace Scripts.Gameplay.Map.Objects
{
    public static class Utils
    {
        public static bool IsInLayerMask(int layer, LayerMask layermask)
        {
            return layermask == (layermask | (1 << layer));
        }
    }
}