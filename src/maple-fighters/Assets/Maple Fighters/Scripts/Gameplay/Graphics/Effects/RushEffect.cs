using UnityEngine;

namespace Scripts.Gameplay.Graphics
{
    public class RushEffect : MonoBehaviour
    {
        [SerializeField]
        private int time = 1;

        private void Start()
        {
            Destroy(gameObject, time);
        }
    }
}