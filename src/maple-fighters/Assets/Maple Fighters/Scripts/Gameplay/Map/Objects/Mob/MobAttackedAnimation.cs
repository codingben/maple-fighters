using System.Collections;
using UnityEngine;

namespace Scripts.Gameplay.Map.Objects
{
    [RequireComponent(typeof(Animator))]
    public class MobAttackedAnimation : MonoBehaviour
    {
        [SerializeField]
        private string animationName = "Hitted";

        [SerializeField]
        private float hittedTime = 1f;

        private Animator animator;
        private Coroutine coroutine;

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        public void Play()
        {
            animator.SetBool(animationName, true);

            if (coroutine != null)
            {
                StopCoroutine(coroutine);
            }

            coroutine = StartCoroutine(WaitAndUnsetHittedState());
        }

        public void Stop()
        {
            animator.SetBool(animationName, false);
        }

        private IEnumerator WaitAndUnsetHittedState()
        {
            yield return new WaitForSeconds(hittedTime);

            animator.SetBool(animationName, false);
        }
    }
}