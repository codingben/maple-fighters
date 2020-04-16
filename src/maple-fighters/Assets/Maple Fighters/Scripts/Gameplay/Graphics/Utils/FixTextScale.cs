using System;
using Game.Common;
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
                var direction = x > 0 ? Directions.Left : Directions.Right;

                switch (direction)
                {
                    case Directions.Left:
                    {
                        previousLocalScale.x = Math.Abs(previousLocalScale.x);
                        break;
                    }

                    case Directions.Right:
                    {
                        previousLocalScale.x = -Math.Abs(previousLocalScale.x);
                        break;
                    }
                }

                transform.localScale = 
                    new Vector3(previousLocalScale.x, previousLocalScale.y);
            }
        }
    }
}