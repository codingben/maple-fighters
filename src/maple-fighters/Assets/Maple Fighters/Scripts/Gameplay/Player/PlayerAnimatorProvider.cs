using UnityEngine;

namespace Scripts.Gameplay.Player
{
    public class PlayerAnimatorProvider : MonoBehaviour
    {
        private Animator animator;

        public void Initialize(Animator animator)
        {
            this.animator = animator;
        }

        public Animator Provide()
        {
            return animator;
        }
    }
}