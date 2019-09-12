using Game.Common;
using UnityEngine;

namespace Assets.Scripts.Gameplay.Utils
{
    [ExecuteInEditMode]
    public class FixTextScale : MonoBehaviour
    {
        [SerializeField]
        private Transform parent;
        private float x;

        private void Awake()
        {
            x = transform.localScale.x;
        }

        private void Update()
        {
            if (parent != null)
            {
                var direction = 
                    parent.localScale.x > 0
                        ? Directions.Left
                        : Directions.Right;

                switch (direction)
                {
                    case Directions.Left:
                    {
                        transform.localScale = 
                            new Vector3(
                                x,
                                transform.localScale.y,
                                transform.localScale.z);
                        break;
                    }

                    case Directions.Right:
                    {
                        transform.localScale = 
                            new Vector3(
                                -x,
                                transform.localScale.y,
                                transform.localScale.z);
                        break;
                    }
                }
            }
        }
    }
}