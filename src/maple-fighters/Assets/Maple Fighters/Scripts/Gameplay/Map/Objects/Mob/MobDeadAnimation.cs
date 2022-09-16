using UnityEngine;

namespace Scripts.Gameplay.Map.Objects
{
    [RequireComponent(typeof(Animator))]
    public class MobDeadAnimation : MonoBehaviour
    {
        [SerializeField]
        private string animationName = "Dead";

        private Animator animator;

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        public void Play()
        {
            animator.SetBool(animationName, true);
        }
    }
}