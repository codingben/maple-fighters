using Game.Common;
using UnityEngine;

namespace Assets.Scripts.Gameplay.Utils
{
    [ExecuteInEditMode]
    public class FixTextScale : MonoBehaviour
    {
        [SerializeField] private Transform parent;
        private float scale;

        private void Awake()
        {
            scale = transform.localScale.x;
        }

        private void Update()
        {
            var direction = GetDirection();
            switch (direction)
            {
                case Directions.Left:
                {
                    transform.localScale = new Vector3(scale, transform.localScale.y, transform.localScale.z);
                    break;
                }
                case Directions.Right:
                {
                    transform.localScale = new Vector3(-scale, transform.localScale.y, transform.localScale.z);
                    break;
                }
            }
        }

        private Directions GetDirection()
        {
            if (parent?.localScale.x > 0)
            {
                return Directions.Left;
            }

            if (parent?.localScale.x < 0)
            {
                return Directions.Right;
            }

            {
                return Directions.None;
            }
        }
    }
}