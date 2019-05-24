using Game.Common;
using Scripts.Utils.Shared;
using UnityEngine;

namespace Scripts.Gameplay.Actors
{
    public class PlayerAnimatorProvider : MonoBehaviour
    {
        private Animator animator;

        public void Set(Animator animator)
        {
            this.animator = animator;
        }

        public Animator Provide()
        {
            return animator;
        }
    }
}