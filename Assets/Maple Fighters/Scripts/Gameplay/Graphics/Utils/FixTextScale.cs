using CommonTools.Log;
using Game.Common;
using UnityEngine;

namespace Assets.Scripts.Gameplay.Utils
{
    [ExecuteInEditMode]
    public class FixTextScale : MonoBehaviour
    {
        [SerializeField]
        private Transform parent;
        private float scale;

        private void Awake()
        {
            if (parent == null)
            {
                LogUtils.Log(
                    MessageBuilder.Trace("Parent is null."),
                    LogMessageType.Warning);
            }

            scale = transform.localScale.x;
        }

        private void Update()
        {
            if (parent != null)
            {
                ChangeDirection();
            }
        }

        private void ChangeDirection()
        {
            switch (GetDirection())
            {
                case Directions.Left:
                {
                    transform.localScale = 
                        new Vector3(
                            scale,
                            transform.localScale.y,
                            transform.localScale.z);
                    break;
                }

                case Directions.Right:
                {
                    transform.localScale = 
                        new Vector3(
                            -scale,
                            transform.localScale.y,
                            transform.localScale.z);
                    break;
                }
            }
        }

        private Directions GetDirection()
        {
            var direction = Directions.None;

            if (parent != null)
            {
                if (parent.localScale.x > 0)
                {
                    direction = Directions.Left;
                }

                if (parent.localScale.x < 0)
                {
                    direction = Directions.Right;
                }
            }

            return direction;
        }
    }
}