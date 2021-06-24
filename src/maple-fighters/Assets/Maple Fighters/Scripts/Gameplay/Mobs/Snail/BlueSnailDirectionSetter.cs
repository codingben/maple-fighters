using Scripts.Gameplay.Entity;
using UnityEngine;

namespace Scripts.Gameplay.Mobs
{
    [RequireComponent(typeof(PositionSetter))]
    public class BlueSnailDirectionSetter : MonoBehaviour
    {
        private PositionSetter positionSetter;

        private void Awake()
        {
            positionSetter = GetComponent<PositionSetter>();
            positionSetter.PositionChanged += OnPositionChanged;
        }

        private void OnPositionChanged(Vector3 position)
        {
            var direction = (transform.position - position).normalized;
            var x = Mathf.Abs(transform.localScale.x);
            var y = transform.localScale.y;
            var z = transform.localScale.z;

            if (direction.x > 0)
            {
                transform.localScale = new Vector3(x, y, z);
            }
            else
            {
                transform.localScale = new Vector3(-x, y, z);
            }
        }
    }
}