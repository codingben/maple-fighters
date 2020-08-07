using System;
using UnityEngine;

namespace Scripts.Gameplay.Graphics
{
    [ExecuteInEditMode]
    public class FixTextScale : MonoBehaviour
    {
        [SerializeField]
        private Transform parent;
        private Vector2 previousLocalScale;

        private void Awake()
        {
            previousLocalScale = transform.localScale;
        }

        private void Update()
        {
            if (parent != null)
            {
                var x = parent.localScale.x;
                if (x > 0)
                {
                    previousLocalScale.x = Math.Abs(previousLocalScale.x);
                }
                else
                {
                    previousLocalScale.x = -Math.Abs(previousLocalScale.x);
                }

                transform.localScale =
                    new Vector3(previousLocalScale.x, previousLocalScale.y);
            }
        }
    }
}