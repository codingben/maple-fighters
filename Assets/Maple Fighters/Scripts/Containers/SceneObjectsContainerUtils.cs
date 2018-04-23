using Game.Common;
using UnityEngine;

namespace Scripts.Containers
{
    internal static class SceneObjectsContainerUtils
    {
        internal static void ChangeDirection(this Transform sceneObject, Directions direction)
        {
            var x = sceneObject.transform.localScale.x;

            switch (direction)
            {
                case Directions.Left:
                {
                    sceneObject.localScale = new Vector3(x, sceneObject.localScale.y, sceneObject.localScale.z);
                    break;
                }
                case Directions.Right:
                {
                    sceneObject.localScale = new Vector3(-x, sceneObject.localScale.y, sceneObject.localScale.z);
                    break;
                }
            }
        }
    }
}